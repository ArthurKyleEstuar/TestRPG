using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class Objective : ScriptableObject
{
    [SerializeField] private string description;

    [SerializeField] private bool isCompleted = false;
    public bool IsCompleted 
    {
        get 
        {
            return isCompleted; 
        }
        private set 
        {
            isCompleted = value; 
        }
    }

    public string Description => description;

    public event System.Action OnProgressUpdated;
    public event System.Action OnObjectiveCompleted;

    public virtual void Init() { }

    protected virtual void CompleteObjective() 
    {
        IsCompleted = true;
        InvokeComplete();
    }

    // Invoke accessor for events
    protected void InvokeUpdate() => OnProgressUpdated?.Invoke();
    protected void InvokeComplete() => OnObjectiveCompleted?.Invoke();
}
