using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    public void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
