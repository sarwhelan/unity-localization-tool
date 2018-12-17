using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LanguageListener))]
[ExecuteInEditMode]
public class TextEditor : Editor {

    int selectedKeyPhrase;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // langListener allows us to call public methods/attributes made available by the LanguageListener class
        LanguageListener langListener = (LanguageListener)target;

        selectedKeyPhrase = langListener.IndexOfCurrentKeyPhrase();
        selectedKeyPhrase = EditorGUILayout.Popup("Key phrase", selectedKeyPhrase, langListener.GetKeyPhraseArray());
        langListener.SetKeyPhrase(selectedKeyPhrase);



    }

}