using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class InteractionManager
{
    private static readonly List<InteractionTrigger> _possibleInteractions = new List<InteractionTrigger>();

    private static InteractionTrigger _currentInteraction = null;
    private static Type _currentInteractionItemType = null;
    private static int _currentInteractionLength = 0;
    private static int _currentInteractionIndex = 0;
    

    public static void AddPossibleInteraction(InteractionTrigger trigger)
    {
        if (trigger.CheckConditions())
        {
            _possibleInteractions.Add(trigger);
            GameObject tag = UnityEngine.Object.Instantiate(
                GameDataManager.resourcesRegistry[("InteractionTag", typeof(GameObject))] as GameObject,
                new Vector3(trigger.transform.position.x, trigger.transform.position.y + 1.0f),
                Quaternion.identity,
                trigger.transform);
        }
    }

    public static void RemovePossibleInteraction(InteractionTrigger trigger)
    {
        for (int i = 0; i < _possibleInteractions.Count; i++)
        {
            if (_possibleInteractions[i] == trigger)
            {
                GameObject interactionTag = trigger.transform.Find("InteractionTag(Clone)")?.gameObject;

                if (interactionTag != null)
                    UnityEngine.Object.Destroy(interactionTag);

                _possibleInteractions.RemoveAt(i);
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
            InteractionTrigger nearestInteraction = _possibleInteractions.FirstOrDefault();
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
            int compareDistance = Vector3.Distance(playerPos, x1.transform.position).CompareTo(Vector3.Distance(playerPos, x2.transform.position));

            if(compareDistance != 0)
                return compareDistance;

            return x1.interactionParams.Priority.CompareTo(x2.interactionParams.Priority);
        });
    }
}