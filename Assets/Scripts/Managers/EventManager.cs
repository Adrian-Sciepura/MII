using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;

    private Dictionary<Type, List<Action<object>>> _eventSubscribers;

    private Queue<Tuple<Type, object>> _eventQueue;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _eventSubscribers = new Dictionary<Type, List<Action<object>>>();
        _eventQueue = new Queue<Tuple<Type, object>>();
    }

    private void Update()
    {
        Tuple<Type, object> currentElement;

        while(_eventQueue.Count > 0)
        {
            currentElement = _eventQueue.Dequeue();
            List<Action<object>> subscribers;

            if (!_eventSubscribers.TryGetValue(currentElement.Item1, out subscribers))
                continue;

            foreach (var subscriber in subscribers)
                subscriber(currentElement.Item2);
        }
    }

    public static void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_instance._eventSubscribers.ContainsKey(eventType))
            _instance._eventSubscribers.Add(eventType, new List<Action<object>>());

        _instance._eventSubscribers[eventType].Add(e => handler((TEvent)e));
    }

    public static void Subscribe(Type eventType, Action<object> handler)
    {
        if (!_instance._eventSubscribers.ContainsKey(eventType))
            _instance._eventSubscribers.Add(eventType, new List<Action<object>>());

        _instance._eventSubscribers[eventType].Add(e => handler(e));
    }

    public static void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_instance._eventSubscribers.ContainsKey(eventType))
            return;

        _instance._eventSubscribers[eventType].Remove(e => handler((TEvent)e));
    }

    public static void Publish<TEvent>(TEvent e)
    {
        Type eventType = typeof(TEvent);
        _instance._eventQueue.Enqueue(new(eventType, e));
    }

    public static void Publish(object e)
    {
        Type eventType = e.GetType();
        _instance._eventQueue.Enqueue(new(eventType, e));
    }

    public static void Publish(Type eventType, object e)
    {
        _instance._eventQueue.Enqueue(new(eventType, e));
    }
}
