using System;
using System.Collections.Generic;

public static class UIEventSystem
{
    private static Dictionary<string, Action> eventTable = new Dictionary<string, Action>();

    public static void Register(string eventName, Action callback)
    {
        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = callback;
        else
            eventTable[eventName] += callback;
    }

    public static void Unregister(string eventName, Action callback)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName] -= callback;
    }

    public static void TriggerEvent(string eventName)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName]?.Invoke();
    }
}