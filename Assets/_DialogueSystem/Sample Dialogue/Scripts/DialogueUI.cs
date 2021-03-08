using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueWindow;

    [SerializeField] private TextMeshProUGUI    speaker;
    [SerializeField] private TextMeshProUGUI    text;

    [SerializeField] private Button             nextButton;
    [SerializeField] private Button             quitButton;

    [SerializeField] private GameObject         textWindow;
    [SerializeField] private GameObject         choiceWindow;

    [SerializeField] private Transform          choiceRoot;
    [SerializeField] private GameObject         choicePrefab;

    private DialogueManager manager = null;

    private void OnEnable()
    {
        if (manager == null) return;
        manager.OnNextDialogue += UpdateUI;
        manager.OnDialogueEnd += Quit;
    }

    private void OnDisable()
    {
        if (manager == null) return;
        manager.OnNextDialogue -= UpdateUI;
        manager.OnDialogueEnd -= Quit;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = DialogueManager.Instance;
        if (manager == null) return;

        manager.OnNextDialogue += UpdateUI;
        manager.OnDialogueEnd += Quit;

        if (nextButton != null)
            nextButton.onClick.AddListener(GetNextDialogue);
        if (quitButton != null)
            quitButton.onClick.AddListener(Quit);

        if (dialogueWindow != null)
            dialogueWindow.SetActive(false);
    }
    
    void GetNextDialogue()
    {
        manager.NextDialogue();
    }

    void UpdateUI()
    {
        if (manager == null) return;

        if (dialogueWindow.activeSelf == false)
            dialogueWindow.SetActive(true);

        if (choiceRoot != null)
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
        }

        textWindow.SetActive(!manager.HasChoices);
        choiceWindow.SetActive(manager.HasChoices);

        if (manager.HasChoices)
        {
            BuildChoiceList();
        }
        else
        {
            speaker.text = manager.GetSpeaker();
            text.text = manager.GetText();
        }
    }

    void Quit()
    {
        dialogueWindow.SetActive(false);
    }

    // Generate choices based on number of child nodes
    private void BuildChoiceList()
    {
        foreach (DialogueNode choice in manager.GetChoices())
        {
            GameObject choiceButton = Instantiate(choicePrefab, choiceRoot);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();

            Button button = choiceButton.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                manager.SelectChoice(choice);
            });
        }
    }
}
