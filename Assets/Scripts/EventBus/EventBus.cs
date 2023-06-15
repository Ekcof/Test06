using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the events for interaction among classes
/// </summary>
public static class EventsBus
{
    private static readonly Dictionary<Type, List<Action<object>>> eventSubscriptions = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (!eventSubscriptions.ContainsKey(eventType))
        {
            eventSubscriptions[eventType] = new List<Action<object>>();
        }

        eventSubscriptions[eventType].Add(obj => eventHandler((T)obj));
    }

    public static void Unsubscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            handlers.RemoveAll(obj => obj.Equals(eventHandler));
        }
        else
        {
            Debug.LogWarning($"Event {eventType.Name} is not subscribed to in the EventBus.");
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventSubscriptions.TryGetValue(eventType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler.Invoke(eventData);
            }
        }
        else
        {
            Debug.LogWarning($"Event {eventType.Name} is not subscribed to in the EventBus.");
        }
    }
}

public class OnItemSlotSelected { public ItemSlot ItemSlot; public Item Item; public int SlotNumber; }
public class OnItemDropped { public ItemHolder ItemHolder; public Item Item; }
public class OnItemPickedUp { public ItemHolder ItemHolder; public Item Item; public IItemTaker Pickable; }

public class OnShutDownWindows { }

public class OnApproachingItemHolder { public ItemHolder ItemHolder; }
public class OnLeavingItemHolder { public ItemHolder ItemHolder; }
public class OnTakeButtonPress {  }
public class OnOpenUIWindow { public string name; }
public class OnHideUIWindow { public bool EnableGeneralHUD; }