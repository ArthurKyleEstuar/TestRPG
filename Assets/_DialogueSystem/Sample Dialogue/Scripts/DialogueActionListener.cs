using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueActionListener : MonoBehaviour
{
    [SerializeField] private string action;

    [SerializeField] private UnityEvent onAction;

    private void Start()
    {
        DialogueManager.Instance.OnAction += OnActionActivated;
    }

    void OnActionActivated(string trigger)
    {
        if (action != trigger) return;

        onAction?.Invoke();
    }
}
