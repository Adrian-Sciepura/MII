using System.Collections.Generic;
using UnityEngine;

public static class InteractionManager
{
    private static InteractionSystem _currentInteraction = null;
    private static int _currentInteractionLength = 0;
    private static int _currentInteractionIndex = 0;

    private static readonly List<GameEntity> _possibleInteractions = new List<GameEntity>();

    public static void AddPossibleInteraction(GameEntity entity)
    {
        if (entity.interactionContainer.GetInteraction() != null)
        {
            _possibleInteractions.Add(entity);
            GameObject tag = UnityEngine.Object.Instantiate(
                GameDataManager.prefabRegistry["InteractionTag"], 
                new Vector3(entity.transform.position.x, entity.transform.position.y + 1.0f), 
                Quaternion.identity, 
                entity.transform);
        }
    }

    public static void RemovePossibleInteraction(GameEntity entity)
    {
        for (int i = 0; i < _possibleInteractions.Count; i++)
        {
            if (_possibleInteractions[i] == entity)
            {
                GameObject interactionTag = entity.transform.Find("InteractionTag(Clone)")?.gameObject;

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
        if (_possibleInteractions.Count == 0 && LevelManager.playerEntity == null)
            return;

        bool succeded = false;

        while (!succeded && _possibleInteractions.Count > 0)
        {
            GameEntity nearestInteraction = LevelManager.FindNearest(LevelManager.playerEntity, _possibleInteractions);
            if (nearestInteraction != null)
            {
                InteractionSystem nearestInteractionSystem = nearestInteraction.interactionContainer.GetInteraction();
                succeded = StartInteraction(nearestInteractionSystem);

                if (!succeded)
                    RemovePossibleInteraction(nearestInteraction);
            }
        }
    }

    public static bool StartInteraction(InteractionSystem interactionSystem)
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
}