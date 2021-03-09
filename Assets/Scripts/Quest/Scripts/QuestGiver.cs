using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteractable
{
    [SerializeField] private string questID;

    void GiveQuest()
    {
        QuestManager.Instance.AcceptQuest(questID);
    }

    public void Interact()
    {
        GiveQuest();
    }
}
