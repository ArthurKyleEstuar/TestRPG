﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/AssetModificationProcessor.html

// Special function to fix dialogue asset/scriptable object renaming bug
public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    {
        Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
        if (dialogue == null)
        {
            return AssetMoveResult.DidNotMove;
        }

        if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
        {
            return AssetMoveResult.DidNotMove;
        }

        dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

        return AssetMoveResult.DidNotMove;
    }
}
