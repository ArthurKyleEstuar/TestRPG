using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TargetCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetCount;

    public void Initialize(int newCount)
    {
        if (targetCount != null)
            targetCount.text = newCount.ToString();
    }
}
