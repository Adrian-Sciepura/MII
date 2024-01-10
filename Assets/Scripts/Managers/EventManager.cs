using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;

    private Dictionary<Type, List<Tuple<Action<object>, string>>> _eventSubscribers;

    private Queue<Tuple<Type, object>> _eventQueue;

    public static void DestroySingleton()
    {
        Destroy(_instance.gameObject);
        _instance = null;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        _eventSubscribers = new Dictionary<Type, List<Tuple<Action<object>, string>>>();
        _eventQueue = new Queue<Tuple<Type, object>>();

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _eventQueue.Clear();
    }

    private void Update()
    {
        Tuple<Type, object> currentElement;

        while(_eventQueue.Count > 0)
        {
            currentElement = _eventQueue.Dequeue();
            List<Tuple<Action<object>, string>> subscribers;

            if (!_eventSubscribers.TryGetValue(currentElement.Item1, out subscribers))
                continue;

            foreach (var subscriber in subscribers)
                subscriber.Item1(currentElement.Item2);
        }
    }

    public static void Subscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_instance._eventSubscribers.ContainsKey(eventType))
            _instance._eventSubscribers.Add(eventType, new List<Tuple<Action<object>, string>>());

        MethodInfo methodInfo = handler.GetMethodInfo();
        _instance._eventSubscribers[eventType].Add(new(e => handler((TEvent)e), $"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}"));
    }

    public static void Subscribe(Type eventType, Action<object> handler)
    {
        if (!_instance._eventSubscribers.ContainsKey(eventType))
            _instance._eventSubscribers.Add(eventType, new List<Tuple<Action<object>, string>>());

        MethodInfo methodInfo = handler.GetMethodInfo();
        _instance._eventSubscribers[eventType].Add(new (handler, $"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}"));
    }

    public static void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if (!_instance._eventSubscribers.ContainsKey(eventType))
            return;

        MethodInfo methodInfo = handler.GetMethodInfo();
        _instance._eventSubscribers[eventType].RemoveAll(x => x.Item2 == $"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}");
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
