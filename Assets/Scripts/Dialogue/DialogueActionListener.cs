using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueActionListener : MonoBehaviour
{
    //[System.Serializable]
    //public struct ActionSet
    //{
    //    public string action;
    //    public UnityEvent onAction;
    //}
    ////public List<ActionSet> actionSets;

    public string action;
    public UnityEvent onAction;

    private void Start()
    {
        DialogueManager.Instance.OnAction += OnActionActivated;
    }

    void OnActionActivated(string trigger)
    {
        if (trigger == action)
            onAction?.Invoke();

        //for (int x = 0; x < actionSets.Count; x++)
        //{
        //    if (actionSets[x].action == trigger)
        //        actionSets[x].onAction?.Invoke();
        //}
    }
}
