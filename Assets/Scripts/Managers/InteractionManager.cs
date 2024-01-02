using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class InteractionManager
{
    private static readonly List<Tuple<InteractionTrigger, GameObject>> _possibleInteractions = new List<Tuple<InteractionTrigger, GameObject>>();

    private static InteractionTrigger _currentInteraction = null;
    private static Type _currentInteractionItemType = null;
    private static int _currentInteractionLength = 0;
    private static int _currentInteractionIndex = 0;
    

    public static void AddPossibleInteraction(InteractionTrigger trigger)
    {
        if (trigger.CheckConditions() && !_possibleInteractions.Any(x => x.Item1 == trigger))
        {
            Transform parentTransform = trigger.transform.parent ?? trigger.transform;
            Collider2D collider = parentTransform.GetComponent<Collider2D>();

            GameObject tag = parentTransform.Find("InteractionTag(Clone)")?.gameObject;

            if (tag == null)
            {
                Vector3 spawnPos = collider != null
                ? new Vector3(parentTransform.position.x, parentTransform.position.y + collider.bounds.size.y * 0.5f)
                : new Vector3(parentTransform.position.x, parentTransform.position.y + 1.0f);

                tag = UnityEngine.Object.Instantiate(
                    GameDataManager.resourcesRegistry[("InteractionTag", typeof(GameObject))] as GameObject,
                    spawnPos,
                    Quaternion.identity,
                    parentTransform);

                tag.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
            
            _possibleInteractions.Add(new Tuple<InteractionTrigger, GameObject>(trigger, tag));
        }
    }

    public static void RemovePossibleInteraction(InteractionTrigger trigger)
    {
        for (int i = 0; i < _possibleInteractions.Count; i++)
        {
            if (_possibleInteractions[i].Item1 == trigger)
            {
                GameObject tag = _possibleInteractions[i].Item2;
                _possibleInteractions.RemoveAt(i);

                if (!_possibleInteractions.Any(x => x.Item2 == tag))
                    UnityEngine.Object.Destroy(tag);
                
                return;
            }
        }
    }

    public static void Setup()
    {
        var interactionItems = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => 
                !t.IsAbstract &&
                t.GetInterfaces().Contains(typeof(IInteractionItem)));


        foreach (var interactionItemType in interactionItems)
        {
            Type eventFinishType = typeof(OnInteractionItemFinishEvent<>).MakeGenericType(interactionItemType);
            EventManager.Instance.Subscribe(eventFinishType, NextInteractionItem);
        }
    }


    public static void StartNearestInteraction()
    {
        if (_possibleInteractions.Count == 0 || LevelManager.playerEntity == null)
            return;

        bool succeded = false;
        SortInteractions();

        while (!succeded && _possibleInteractions.Count > 0)
        {
            InteractionTrigger nearestInteraction = _possibleInteractions.FirstOrDefault().Item1;
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
        if (_currentInteraction != null || interactionSystem.content.Count == 0)
            return false;

        _currentInteraction = interactionSystem;
        _currentInteractionLength = _currentInteraction.content.Count;
        _currentInteractionIndex = 0;

        EventManager.Instance.Publish(new OnInteractionStartEvent(_currentInteraction));
        StartInteractionItem();

        return true;
    }

    private static void NextInteractionItem(object e)
    {
        if (_currentInteraction == null)
            return;

        if(_currentInteractionIndex >= _currentInteractionLength - 1)
        {
            EventManager.Instance.Publish(new OnInteractionFinishEvent());
            _currentInteraction = null;
            return;
        }

        _currentInteractionIndex++;
        StartInteractionItem();
    }

    private static void StartInteractionItem()
    {
        _currentInteractionItemType = _currentInteraction.content[_currentInteractionIndex].GetType();
        Type eventStartType = typeof(OnInteractionItemStartEvent<>).MakeGenericType(_currentInteractionItemType);
        EventManager.Instance.Publish(eventStartType, Activator.CreateInstance(eventStartType, new object[] { _currentInteraction.content[_currentInteractionIndex] }));
    }

    private static void SortInteractions()
    {
        Vector3 playerPos = LevelManager.playerEntity.transform.position;

        _possibleInteractions.Sort((x1, x2) =>
        {
            int compareDistance = Vector3.Distance(playerPos, x2.Item1.transform.position).CompareTo(Vector3.Distance(playerPos, x1.Item1.transform.position));

            if(compareDistance != 0)
                return compareDistance;

            return x2.Item1.interactionParams.Priority.CompareTo(x1.Item1.interactionParams.Priority);
        });
    }
}