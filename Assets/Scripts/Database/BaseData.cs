using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData
{
    [SerializeField] protected string id;

    public string ID => id;

    public void SetID(string id) => this.id = id;
}
