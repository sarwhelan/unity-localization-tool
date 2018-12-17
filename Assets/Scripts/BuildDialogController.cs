using UnityEngine;
using System.Collections;
using UnityEditor;

// BuildDialogController is used to create a new Dialog Tool asset in the case that one does not yet exist
[ExecuteInEditMode]
public class BuildDialogController
{

    public static DialogController BuildAsset()
    {
        DialogController asset = ScriptableObject.CreateInstance<DialogController>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Dialog Tool.asset");
        AssetDatabase.SaveAssets();

        return asset;
    }
}