using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum RotateMode
{
    _0      = 0,
    _90     = 1,
    _180    = 2,
    _270    = 3,
}
public class MapEditorWindow : EditorWindow
{
    public TileType         tileToPaint;
    public RotateMode       tileRotation;
    private bool            firstLoad = true;
    private TileDatabase    tileDB;

    private static bool willPaint;

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

        LoadFirstSelectedTile();

        firstLoad = false;
    }

    private void OnDisable()
    {
        firstLoad = true;
        SceneView.duringSceneGui -= SceneGUI;
    }

    private void LoadFirstSelectedTile()
    {
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
    }

    TileType prevType;
    RotateMode prevRot;
    private void OnGUI()
    {
        GUILayout.Space(20);

        if (firstLoad) return;

        tileToPaint = (TileType)EditorGUILayout.EnumPopup("New Tile Type", tileToPaint);
        tileRotation = (RotateMode)EditorGUILayout.EnumPopup("New Tile Rotation", tileRotation);

        if (prevType != tileToPaint || prevRot != tileRotation)
        {
            prevType = tileToPaint;
            prevRot = tileRotation;
            UpdateTile();
        }

        GUILayout.Space(10);

        willPaint = GUILayout.Toggle(willPaint, "Paint On Click Mode");

        if (GUILayout.Button("Paint tiles"))
            UpdateTile(true);

        if (GUILayout.Button("Deselect tiles"))
            Selection.objects = new Object[0];

        GeneratePreview();
    }

    private void GeneratePreview()
    {
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

        EditorGUI.PrefixLabel(new Rect(25, 148, 100, 5), new GUIContent("Sprite preview"));
        EditorGUI.DrawPreviewTexture(new Rect(25, 150, 100, 100), tex);
    }

    private void UpdateTile(bool overridePaintCheck = false)
    {
        if (Selection.gameObjects.Length <= 0) return;

        if (!overridePaintCheck && !willPaint) return;
       
        foreach (GameObject go in Selection.gameObjects)
        {
            MapTileController mtc = go.GetComponent<MapTileController>();

            if (mtc == null) continue;

            Vector3 rotAngle = Vector3.zero;

            string tileRotMode = tileRotation.ToString().Replace("_", "");

            float newZAngle = 0;

            float.TryParse(tileRotMode, out newZAngle);

            rotAngle.z = newZAngle;

            mtc.SetTileType(tileToPaint, rotAngle);
        }
    }

    private void SceneGUI(SceneView sceneView)
    {
        switch(Event.current.type)
        {
            case EventType.Used:
                {
                    UpdateTile();
                    break;
                }
        }
    }

}


