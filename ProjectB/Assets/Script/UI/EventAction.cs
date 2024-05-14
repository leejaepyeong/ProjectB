using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventAction
{
    static readonly Dictionary<eEventKey, Action> dicEvents = new();

    public static void RemoveAllListner()
    {
        foreach (var ele in dicEvents.Keys)
        {
            if (dicEvents.TryGetValue(ele, out var action))
                action = null;
        }
        dicEvents.Clear();
    }

    public static void AddListner(eEventKey key, Action action)
    {
        if (dicEvents.ContainsKey(key))
        {
            dicEvents[key] += action;
            return;
        }
        dicEvents.Add(key, action);
    }

    public static void RemoveListner(eEventKey key)
    {
        dicEvents[key] = null;
    }

    public static void ExcuteEvent(eEventKey key)
    {
        if (!dicEvents.TryGetValue(key, out var action))
            return;
        action?.Invoke();
    }
}
