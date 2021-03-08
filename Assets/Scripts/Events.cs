using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event System.Action<string> OnEventFired;

    public static event System.Action<string> OnItemPickedUp;

    public static void PickupItem(string id)
    {
        OnItemPickedUp?.Invoke(id);
    }

    public static void FireEvent(string trigger)
    {
        OnEventFired?.Invoke(trigger);
    }
}
