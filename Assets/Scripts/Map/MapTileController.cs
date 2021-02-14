using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Plains      = 0,
    Road        = 1,
    RoadGrass   = 2,
    RockFace    = 3,
}

[SelectionBase]
public class MapTileController : MonoBehaviour
{
    [SerializeField] private TileDatabase   tileDB;
    [SerializeField] private SpriteRenderer tileRender;
    [SerializeField] private TileType       currTileType;
    [SerializeField] private Collider       tileCollider;

    public string CurrTileTypeId
    {
        get
        {
            return currTileType.ToString().ToLower();
        }
    }
    public TileType CurrTileType => currTileType;

    public void UpdateTileType()
    {
        if (tileRender == null || tileDB == null) return;

        TileData newTileData = tileDB.GetFile(currTileType.ToString().ToLower());

        tileRender.sprite = newTileData.TileSprite;

        if (tileCollider != null)
            tileCollider.isTrigger = !newTileData.IsBlocker;
    }

    public void SetTileType(TileType type, Vector3 tileRot)
    {
        currTileType = type;

        UpdateTileType();

        this.transform.localEulerAngles = tileRot;
    }

    public void RotateTile(bool isClockwise)
    {
        Vector3 newRot = this.transform.localEulerAngles;

        newRot.z += (isClockwise) ? 90 : -90;

        this.transform.localEulerAngles = newRot;
    }

    private void OnValidate()
    {
        UpdateTileType();
    }
}
