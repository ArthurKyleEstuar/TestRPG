using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;

    public virtual void Interact()
    {
        onInteract?.Invoke();
    }

    public void StartQuest(string id)
    {
        if (id == "") return;
        QuestManager.Instance.AcceptQuest(id);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null) return;
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void CollectItem(string id)
    {
        if (id == "") return;
        Events.PickupItem(id);
        Destroy(gameObject);
    }
}
