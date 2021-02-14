using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDatabase<T> : ScriptableObject where T : BaseData
{
    [SerializeField] private List<T> data;

    public T GetFile(string id)
    {
        return data.Find(obj => obj.ID == id);
    }
}
