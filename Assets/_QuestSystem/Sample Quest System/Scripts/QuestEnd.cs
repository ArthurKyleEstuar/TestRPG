using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnd : MonoBehaviour, IInteractable
{
    [SerializeField] private string eventToFire;
    public void Interact()
    {
        FinishQuest();
    }

    void FinishQuest()
    {
        Events.FireEvent(eventToFire);
    }
}
