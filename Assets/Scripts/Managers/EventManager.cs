using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private readonly Dictionary<Type, List<Action<object>>> _eventSubscribers = new Dictionary<Type, List<Action<object>>>();

    private readonly Queue<Tuple<Type, object>> _eventQueue = new Queue<Tuple<Type, object>>();

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
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

    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_eventSubscribers.ContainsKey(eventType))
            _eventSubscribers.Add(eventType, new List<Action<object>>());

        _eventSubscribers[eventType].Add(e => handler((TEvent)e));
    }

    public void Subscribe(Type eventType, Action<object> handler)
    {
        if (!_eventSubscribers.ContainsKey(eventType))
            _eventSubscribers.Add(eventType, new List<Action<object>>());

        _eventSubscribers[eventType].Add(e => handler(e));
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_eventSubscribers.ContainsKey(eventType))
            return;

        _eventSubscribers[eventType].Remove(e => handler((TEvent)e));
    }

    public void Publish<TEvent>(TEvent e)
    {
        Type eventType = typeof(TEvent);
        _eventQueue.Enqueue(new(eventType, e));
    }

    public void Publish(object e)
    {
        Type eventType = e.GetType();
        _eventQueue.Enqueue(new(eventType, e));
    }

    public void Publish(Type eventType, object e)
    {
        _eventQueue.Enqueue(new(eventType, e));
    }
}
