using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGridGenerator))]
public class MapGridEditor : Editor
{
    private MapGridGenerator mapGrid;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        mapGrid = (MapGridGenerator)target;

        if(GUILayout.Button("Spawn Map Grid"))
        {
            if (mapGrid == null) return;

            mapGrid.SpawnMapGrid();
        }

        if (GUILayout.Button("Delete Map Grid"))
        {
            if (mapGrid == null) return;

            mapGrid.DeleteMapGrid();
        } 
    }
}
