using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<IGlobalSubscriber>> _subscribers = new Dictionary<Type, List<IGlobalSubscriber>>();
    private static readonly Dictionary<Type, List<Type>> _cachedSubscriberTypes = new Dictionary<Type, List<Type>>();
    
    public static void Subscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
        foreach (var type in subscriberTypes)
        {
            if (!_subscribers.ContainsKey(type)) _subscribers[type] = new List<IGlobalSubscriber>();
            _subscribers[type].Add(subscriber);
        }
    }

    public static void Unsubscribe(IGlobalSubscriber subscriber)
    {
        List<Type> subscriberTypes = GetSubscriberTypes(subscriber);
        foreach (var type in subscriberTypes)
        {
            if (_subscribers.ContainsKey(type)) _subscribers[type].Remove(subscriber);
        }
    }

    public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action) where TSubscriber : class, IGlobalSubscriber
    {
        List<IGlobalSubscriber> subscribers = _subscribers[typeof(TSubscriber)];
        foreach (var subscriber in subscribers)
        {
            try
            {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }

    private static List<Type> GetSubscriberTypes(IGlobalSubscriber globalSubscriber)
    {
        Type type = globalSubscriber.GetType();
        if (_cachedSubscriberTypes.ContainsKey(type)) return _cachedSubscriberTypes[type];

        List<Type> subscriberTypes = type.
            GetInterfaces()
            .Where(t => t.GetInterfaces().
                Contains(typeof(IGlobalSubscriber))).
            ToList();

        _cachedSubscriberTypes[type] = subscriberTypes;
        return subscriberTypes;
    }
}