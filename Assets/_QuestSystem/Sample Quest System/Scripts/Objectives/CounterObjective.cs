using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Counter")]
public class CounterObjective : Objective
{
    [SerializeField] private string itemID;
    [SerializeField] private float maxGoal;

    public float CurGoal { get; private set; } = 0;

    public float MaxGoal => maxGoal;

    public override void Init()
    {
        CurGoal = 0;
        Events.OnItemPickedUp += IncreaseProgress;
    }

    void IncreaseProgress(string id)
    {
        if (id != itemID) return;

        CurGoal++;
        CurGoal = Mathf.Clamp(CurGoal, 0, maxGoal);

        InvokeUpdate();
        if (IsGoalReached()) CompleteObjective();
    }

    private bool IsGoalReached()
    {
        if (CurGoal >= maxGoal) return true;
        return false;  
    }
}
