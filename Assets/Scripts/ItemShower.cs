using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShower : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private string questID;
    // Start is called before the first frame update
    void Start()
    {
        objects.ForEach(_ => _.SetActive(false));
        QuestManager.Instance.OnQuestAccepted += ShowItems;
    }

    void ShowItems(string id)
    {
        if (id != questID) return;
        objects.ForEach(_ => _.SetActive(true));
    }
}
