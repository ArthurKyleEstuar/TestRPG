using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnd : Interactable
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
