using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private readonly List<Tuple<InteractionTrigger, GameObject>> _possibleInteractions = new List<Tuple<InteractionTrigger, GameObject>>();

    private static InteractionManager _instance;
    private InteractionTrigger _currentInteraction = null;
    private Type _currentInteractionItemType = null;
    private int _currentInteractionLength = 0;
    private int _currentInteractionIndex = 0;
    private Coroutine _currentCoroutine;

    private List<InteractionItem> _subInteraction;
    private int _subInteractionIndex = 0;
    private bool _isSubInteractionRunning = false;

    public static void DestroySingleton()
    {
        if (_instance == null)
            return;

        Destroy(_instance.gameObject);
        _instance = null;
    }
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        var interactionItems = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                t.IsSubclassOf(typeof(InteractionItem)));


        foreach (var interactionItemType in interactionItems)
        {
            Type eventFinishType = typeof(OnInteractionItemFinishEvent<>).MakeGenericType(interactionItemType);
            EventManager.Subscribe(eventFinishType, NextInteractionItem);
        }

        EventManager.Subscribe<OnInteractionFinishEvent>(NextInteractionItem);
    }

    private void Start()
    {
        _possibleInteractions.Clear();
        _currentInteraction = null;
        _currentInteractionLength = 0;
        _currentInteractionIndex = 0;
        _isSubInteractionRunning = false;
        _currentCoroutine = null;
    }

    public static void AddPossibleInteraction(InteractionTrigger trigger)
    {
        if (trigger.CheckConditions() && !_instance._possibleInteractions.Any(x => x.Item1 == trigger))
        {
            Transform parentTransform = trigger.transform.parent ?? trigger.transform;
            SpriteRenderer spriteRenderer = parentTransform.GetComponent<SpriteRenderer>();


            if(trigger.interactionParams.instantPlay)
            {
                StartInteraction(trigger);
                return;
            }


            GameObject tag = parentTransform.Find("InteractionTag(Clone)")?.gameObject;

            if (tag == null)
            {
                GameObject prefab = GameDataManager.resourcesRegistry[("InteractionTag", typeof(GameObject))] as GameObject;

                tag = Instantiate(prefab, parentTransform);
                tag.transform.rotation = Quaternion.identity;
                tag.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);


                tag.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }

            _instance._possibleInteractions.Add(new Tuple<InteractionTrigger, GameObject>(trigger, tag));
        }
    }

    public static void RemovePossibleInteraction(InteractionTrigger trigger)
    {
        for (int i = 0; i < _instance._possibleInteractions.Count; i++)
        {
            if (_instance._possibleInteractions[i].Item1 == trigger)
            {
                GameObject tag = _instance._possibleInteractions[i].Item2;
                _instance._possibleInteractions.RemoveAt(i);

                if (!_instance._possibleInteractions.Any(x => x.Item2 == tag))
                    UnityEngine.Object.Destroy(tag);
                
                return;
            }
        }
    }

    public static void StartNearestInteraction()
    {
        if (_instance._possibleInteractions.Count == 0 || LevelManager.PlayerEntity == null || _instance._currentCoroutine != null)
            return;

        bool succeded = false;
        _instance.SortInteractions();

        while (!succeded && _instance._possibleInteractions.Count > 0)
        {
            InteractionTrigger nearestInteraction = _instance._possibleInteractions.FirstOrDefault().Item1;
            if (nearestInteraction != null)
            {
                if (StartInteraction(nearestInteraction))
                    succeded = true;
                else
                    RemovePossibleInteraction(nearestInteraction);
            }
        }
    }

    public static bool StartInteraction(InteractionTrigger interactionSystem)
    {
        if (_instance._currentInteraction != null || interactionSystem.content.Count == 0)
            return false;

        _instance._currentInteraction = interactionSystem;
        _instance._currentInteractionLength = _instance._currentInteraction.content.Count;
        _instance._currentInteractionIndex = 0;

        foreach (var item in _instance._possibleInteractions)
            if(item.Item2.activeSelf)
                item.Item2.SetActive(false);

        EventManager.Publish(new OnInteractionStartEvent(_instance._currentInteraction));
        _instance.StartInteractionItem(_instance._currentInteraction.content[_instance._currentInteractionIndex]);

        return true;
    }

    public static void StartSubInteraction(List<InteractionItem> interactions)
    {
        if (interactions.Count == 0)
            return;

        _instance._isSubInteractionRunning = true;
        _instance._subInteractionIndex = -1;
        _instance._subInteraction = interactions;
    }

    private void NextInteractionItem(object e)
    {
        if (_currentInteraction == null)
            return;

        if(_isSubInteractionRunning)
        {
            NextSubInteractionItem(e);
            return;
        }

        if(_currentInteractionIndex >= _currentInteractionLength - 1)
        {
            if (_currentInteraction.interactionParams.destroyAfterPlay)
                Destroy(_currentInteraction.gameObject);

            _currentInteraction = null;

            foreach (var item in _possibleInteractions)
                if (!item.Item2.activeSelf)
                    item.Item2.SetActive(true);

            EventManager.Publish(new OnInteractionFinishEvent());
            return;
        }

        _currentInteractionIndex++;
        StartInteractionItem(_currentInteraction.content[_currentInteractionIndex]);
    }

    private void NextSubInteractionItem(object e)
    {
        _subInteractionIndex++;

        if (_subInteractionIndex >= _subInteraction.Count)
        {
            _isSubInteractionRunning = false;
            _subInteraction = null;
            EventManager.Publish(new OnInteractionFinishEvent());
            return;
        }
        
        StartInteractionItem(_subInteraction[_subInteractionIndex]);
    }

    private void StartInteractionItem(InteractionItem item)
    {
        if (item.delay > 0)
            _currentCoroutine = StartCoroutine(StartInteractionWithDelay(item));
        else
            StartInteractionItemNow(item);
    }

    private IEnumerator StartInteractionWithDelay(InteractionItem item)
    {
        yield return new WaitForSeconds(item.delay);
        StartInteractionItemNow(item);
        _currentCoroutine = null;
    }

    private void StartInteractionItemNow(InteractionItem item)
    {
        Type interactionType = item.GetType();
        Type eventStartType = typeof(OnInteractionItemStartEvent<>).MakeGenericType(interactionType);
        EventManager.Publish(eventStartType, Activator.CreateInstance(eventStartType, new object[] { item }));
    }

    private void SortInteractions()
    {
        Vector3 playerPos = LevelManager.PlayerEntity.transform.position;

        _possibleInteractions.Sort((x1, x2) =>
        {
            int compareDistance = Vector3.Distance(playerPos, x2.Item1.transform.position).CompareTo(Vector3.Distance(playerPos, x1.Item1.transform.position));

            if(compareDistance != 0)
                return compareDistance;

            return x2.Item1.interactionParams.Priority.CompareTo(x1.Item1.interactionParams.Priority);
        });
    }
}