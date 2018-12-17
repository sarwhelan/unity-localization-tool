using UnityEngine;
using System.Collections;
using UnityEditor;

// BuildLanguageController is used to create a new Localization Tool asset in the case that one does not yet exist
[ExecuteInEditMode]
public class BuildLanguageController {

    public static LanguageController BuildAsset()
    {
        LanguageController asset = ScriptableObject.CreateInstance<LanguageController>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Localization Tool.asset");
        AssetDatabase.SaveAssets();

        return asset;
    }
}

