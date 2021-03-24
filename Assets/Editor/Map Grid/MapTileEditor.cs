using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapTileController)), CanEditMultipleObjects]
public class MapTileEditor : Editor
{
    private MapTileController tileController;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        tileController = (MapTileController)target;

        if (GUILayout.Button("Update Tile"))
        {
            if (tileController == null) return;

            tileController.UpdateTileType();
        }

        if (GUILayout.Button("Rotate Tile"))
        {
            if (tileController == null) return;

            tileController.RotateTile(true);
        }

        if (GUILayout.Button("Open Editor"))
        {
            MapEditorWindow mew = ScriptableObject.CreateInstance<MapEditorWindow>();
            mew.Show();
        }
    }
}
