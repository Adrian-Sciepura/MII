using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<Type, List<Action<object>>> _eventSubscribers = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_eventSubscribers.ContainsKey(eventType))
            _eventSubscribers.Add(eventType, new List<Action<object>>());

        _eventSubscribers[eventType].Add(e => handler((TEvent)e));
    }

    public static void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_eventSubscribers.ContainsKey(eventType))
            return;

        _eventSubscribers[eventType].Remove(e => handler((TEvent)e));
    }

    public static void Publish<TEvent>(TEvent e)
    {
        Type eventType = typeof(TEvent);

        if (!_eventSubscribers.ContainsKey(eventType))
            return;

        foreach (var subscriber in _eventSubscribers[eventType])
            subscriber(e);
    }
}
