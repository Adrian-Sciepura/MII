using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InteractionManager
{
    private static InteractionTrigger _currentInteraction = null;
    private static int _currentInteractionLength = 0;
    private static int _currentInteractionIndex = 0;

    private static readonly List<InteractionTrigger> _possibleInteractions = new List<InteractionTrigger>();

    public static void AddPossibleInteraction(InteractionTrigger trigger)
    {
        if (trigger.CheckConditions())
        {
            _possibleInteractions.Add(trigger);
            GameObject tag = UnityEngine.Object.Instantiate(
                GameDataManager.prefabRegistry["InteractionTag"],
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
                    Object.Destroy(interactionTag);

                _possibleInteractions.RemoveAt(i);
                return;
            }
        }
    }

    public static void Setup()
    {
        EventManager.Subscribe<OnHighPriorityUpdateEvent>(UpdateInteraction);
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
        _currentInteraction.content[0].Perform();
        EventManager.Publish(new OnInteractionStartEvent());
        return true;
    }

    private static void UpdateInteraction(OnHighPriorityUpdateEvent updateEvent)
    {
        if (_currentInteraction == null)
            return;

        IInteractionItem currentItem = _currentInteraction.content[_currentInteractionIndex];

        if (!currentItem.Finished())
        {
            currentItem.Update();
            return;
        }
        else if (_currentInteractionIndex == _currentInteractionLength - 1)
        {
            _currentInteraction = null;
            EventManager.Publish(new OnInteractionEndEvent());
            return;
        }

        _currentInteractionIndex++;

        if (_currentInteractionIndex != _currentInteractionLength)
            _currentInteraction.content[_currentInteractionIndex].Perform();
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