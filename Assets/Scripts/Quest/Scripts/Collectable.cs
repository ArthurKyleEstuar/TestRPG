using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private string id;
    public void Interact()
    {
        CollectItem();
    }

    void CollectItem()
    {
        Events.PickupItem(id);
        Destroy(gameObject);
    }
}
