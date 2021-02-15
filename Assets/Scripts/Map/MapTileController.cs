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
    [Header("Components")]
    [SerializeField] private TileDatabase   tileDB;
    [SerializeField] private SpriteRenderer tileRender;
    [SerializeField] private Collider       tileCollider;
    [SerializeField] private Vector2        worldCoordinate;
    [SerializeField] private MapGridGenerator gridGen;

    [Header("Settings")]
    [SerializeField] private TileType       currTileType;
    
    public string CurrTileTypeId
    {
        get
        {
            return currTileType.ToString().ToLower();
        }
    }
    public TileType CurrTileType => currTileType;
    public Vector2 WorldCoordinate => worldCoordinate;

    public Vector2 Initialize(MapGridGenerator newGrid)
    {
        gridGen = newGrid;
        return worldCoordinate = this.transform.position;
    }

    public void UpdateTileType(bool isLoad = false)
    {
        if (tileRender == null || tileDB == null) return;

        TileData newTileData = tileDB.GetFile(currTileType.ToString().ToLower());

        tileRender.sprite = newTileData.TileSprite;

        if (tileCollider != null)
            tileCollider.isTrigger = !newTileData.IsBlocker;

        if(!isLoad && gridGen != null)
            gridGen.SaveGrid();
    }

    public void SetTileType(TileType type, Vector3 tileRot, bool isLoad = false)
    {
        currTileType = type;

        UpdateTileType(isLoad);

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
