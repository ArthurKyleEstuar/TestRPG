using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData : BaseData
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private bool   isBlocker;

    public Sprite TileSprite => tileSprite;
    public bool IsBlocker => isBlocker;
}
