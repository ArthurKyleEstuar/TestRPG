using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum RotateMode
{
    _0 = 0,
    _90 = 1,
    _180 = 2,
    _270 = 3,
}
public class MapEditorWindow : EditorWindow
{
    public TileType tileToPaint;
    public RotateMode tileRotation;
    private bool firstLoad = true;
    private TileDatabase tileDB;

    [MenuItem("MAP/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MapEditorWindow));
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += SceneGUI;
        tileDB = AssetDatabase.LoadAssetAtPath<TileDatabase>("Assets/Database/TileDB.asset");

        if (Selection.gameObjects.Length <= 0)
        {
            firstLoad = false;
            return;
        }

        MapTileController mtc = Selection.gameObjects[0].GetComponent<MapTileController>();

        if (mtc == null)
        {
            firstLoad = false;
            return;
        }

        float objectAngle = mtc.gameObject.transform.localEulerAngles.z;

        if (objectAngle >= 270)
            tileRotation = RotateMode._270;
        else if (objectAngle >= 180)
            tileRotation = RotateMode._180;
        else if (objectAngle >= 90)
            tileRotation = RotateMode._90;

        tileToPaint = mtc.CurrTileType;

        firstLoad = false;
    }

    private void OnDisable()
    {
        firstLoad = true;
        SceneView.duringSceneGui -= SceneGUI;
    }

    TileType prevType;
    RotateMode prevRot;
    private void OnGUI()
    {
        if (firstLoad) return;

        tileToPaint = (TileType)EditorGUILayout.EnumPopup("New Tile Type", tileToPaint);
        tileRotation = (RotateMode)EditorGUILayout.EnumPopup("New Tile Rotation", tileRotation);

        if (prevType != tileToPaint || prevRot != tileRotation)
        {
            prevType = tileToPaint;
            prevRot = tileRotation;
            UpdateTile();
        }

        string previewId = tileToPaint.ToString().ToLower();
        Sprite tileSprite = tileDB.GetFile(previewId).TileSprite;

        var rect = tileSprite.rect;
        var tex = new Texture2D((int)rect.width, (int)rect.height);

        var data = tileSprite.texture.GetPixels((int)rect.x
            , (int)rect.y
            , (int)rect.width
            , (int)rect.height);

        tex.SetPixels(data);
        tex.Apply();

        if(GUILayout.Button("Deselect tiles"))
        {
            Selection.objects = new Object[0];
        }

        EditorGUI.PrefixLabel(new Rect(25, 98, 100, 5), new GUIContent("Sprite preview"));
        EditorGUI.DrawPreviewTexture(new Rect(25, 100, 100, 100), tex);
    }

    private void UpdateTile()
    {
        if (Selection.gameObjects.Length <= 0) return;

        MapTileController mtc = Selection.gameObjects[0].GetComponent<MapTileController>();

        if (mtc == null) return;

        Vector3 rotAngle = Vector3.zero;

        switch(tileRotation)
        {
            case RotateMode._0:
                rotAngle.z = 0;
                break;

            case RotateMode._90:
                rotAngle.z = 90;
                break;

            case RotateMode._180:
                rotAngle.z = 180;
                break;

            case RotateMode._270:
                rotAngle.z = 270;
                break;
        }
        mtc.SetTileType(tileToPaint, rotAngle);
    }

    private void SceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.Used)
        {
            UpdateTile();
        }
    }

}


