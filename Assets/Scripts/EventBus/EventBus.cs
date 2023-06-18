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

// Event on selecting of certain item in the inventory of player
public class OnItemSlotSelected { public ItemSlot ItemSlot; public Item Item; public int SlotNumber; }

/// <summary>
/// Event for Item's dropped from the inventory to a new or existing item holder
/// </summary>
public class OnItemDropped { public ItemHolder ItemHolder; public Item Item; }

/// <summary>
/// Event for picking up the item from a certain item holder
/// </summary>
public class OnItemPickedUp { public ItemHolder ItemHolder; public Item Item; public IItemTaker Pickable; }

/// <summary>
/// Event on shutting down all UI windows
/// </summary>
public class OnShutDownWindows { }

/// <summary>
/// Event on approaching some item holder by player
/// </summary>
public class OnApproachingItemHolder { public ItemHolder ItemHolder; }

/// <summary>
/// Event on leaving pickable zone of some item holder by player
/// </summary>
public class OnLeavingItemHolder { public ItemHolder ItemHolder; }

/// <summary>
/// Event on pressing "take" button by player
/// </summary>
public class OnTakeButtonPress {  }

/// <summary>
/// Event on Opening the UI Window by player with a certain name
/// </summary>
public class OnOpenUIWindow { public string name; }

/// <summary>
/// Event on closing the UI window by player with a possibility to disable general HUD
/// </summary>
public class OnHideUIWindow { public bool EnableGeneralHUD; }