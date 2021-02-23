using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparentOnCollide : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer>   renderToAdjust      = new List<SpriteRenderer>();
    [SerializeField] private float                  newAlpha            = 0.2f;
    [SerializeField] private float                  alphaTransitionTime = 0.5f;

    //TODO: Use DG.Tweening
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        foreach (SpriteRenderer sr in renderToAdjust)
        {
            Color newColor = sr.color;

            newColor.a = newAlpha;

            sr.color = newColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        foreach (SpriteRenderer sr in renderToAdjust)
        {
            Color newColor = sr.color;

            newColor.a = 1;

            sr.color = newColor;
        }
    }
}
