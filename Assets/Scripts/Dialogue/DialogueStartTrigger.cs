using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue dialogue;

    public void Interact()
    {
        StartDialogue();
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
