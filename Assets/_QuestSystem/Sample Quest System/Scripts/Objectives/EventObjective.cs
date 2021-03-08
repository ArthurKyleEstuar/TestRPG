using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Event")]
public class EventObjective : Objective
{
    [SerializeField] private string eventToCheck;

    public override void Init()
    {
        Events.OnEventFired += OnEventFired;
    }

    private void OnEventFired(string firedEvent) 
    {
        if (firedEvent != eventToCheck) return;

        CompleteObjective();
    }

}
