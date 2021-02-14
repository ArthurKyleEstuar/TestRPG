using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MapGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private float      tileSize        = 1;
    [SerializeField] private Vector2    gridDimensions  = new Vector2(5, 5);
    [SerializeField] private bool       autoCenter      = true;

    public void SpawnMapGrid()
    {
        for(int x = 0; x < gridDimensions.x; x++)
        {
            for(int y = 0; y < gridDimensions.y; y++)
            {
                GameObject go = Instantiate(mapTilePrefab
                    , this.transform);

                Vector2 spawnPos = new Vector2(x * tileSize
                    , y * tileSize);

                go.transform.localPosition = spawnPos;
                go.transform.localScale = new Vector3(tileSize, tileSize, 1);
            }
        }

        if(autoCenter)
        {
            Vector2 newGridPos = new Vector2(GetGridPoint(gridDimensions.x)
                , GetGridPoint(gridDimensions.y));

            this.transform.position = newGridPos;
        }
    }

    private float GetGridPoint(float gridAxisPoint)
    {
        return -(gridAxisPoint / 2 - 0.5f) * tileSize; 
    }

    public void DeleteMapGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

#if UNITY_EDITOR
    [SerializeField, HideInInspector] float prevTileSize;
    [SerializeField, HideInInspector] Vector2 prevGridSize;
    private void OnValidate()
    {
        if (EditorApplication.isPlaying) return;

        //Apparently editor doesnt like the inverse and cleaner condition, need to do per var ew.
        if (prevTileSize != tileSize && tileSize != 0)
        {
            prevTileSize = tileSize;
            AdjustGrid();
        }

        if(prevGridSize != gridDimensions && gridDimensions.magnitude != 0)
        {
            prevGridSize = gridDimensions;
            AdjustGrid();
        }
    }

    private void AdjustGrid()
    {
        //This is scary. This should be illegal. This shouldnt work but it does.
        EditorApplication.delayCall += () =>
        {
            DeleteMapGrid();
            SpawnMapGrid();
        };
    }
#endif
}
