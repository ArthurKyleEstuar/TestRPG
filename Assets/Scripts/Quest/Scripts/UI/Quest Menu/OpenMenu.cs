using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] private KeyCode openWindowKey = KeyCode.Q;

    [SerializeField] private GameObject questMenu;

    private void Start()
    {
        questMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(openWindowKey))
            questMenu.SetActive(!questMenu.activeSelf);
    }
}
