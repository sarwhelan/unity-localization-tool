using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LanguageController))]
[ExecuteInEditMode]
public class LanguageEditor : Editor {

    string newLang = "";
    string newKeyPhrase = "";
    Dictionary<string, string> dict;
    bool changedEditingLang = true;
    bool addedKey = true;

    List<string> keyPhraseList = new List<string>();
    Dictionary<string, string> editableDict = new Dictionary<string, string>();
    int selectedLang;
    int editingLang;
    int newEditingLang;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // langCtrl enables us to call public methods of LanguageController
        LanguageController langCtrl = (LanguageController)target;

        // language selection
        selectedLang = langCtrl.IndexOfCurrentLang();
        selectedLang = EditorGUILayout.Popup("Current language", selectedLang, langCtrl.GetLangs());
        langCtrl.SelectLang(selectedLang);

        // add new language segment
        GUILayout.BeginHorizontal();
        newLang = EditorGUILayout.TextField("New language", newLang);
        if (GUILayout.Button("Add lang"))
        {
            langCtrl.AddNewLang(newLang);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // add key-phrase
        GUILayout.BeginHorizontal();
        newKeyPhrase = EditorGUILayout.TextField("New key-phrase", newKeyPhrase);
        if (GUILayout.Button("Add key-phrase"))
        {
            langCtrl.AddNewKey(newKeyPhrase);
            addedKey = true;
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // edit language segment: remove the selected lang, add keys, modify values
        editingLang = langCtrl.IndexOfLangBeingEdited();
        newEditingLang = EditorGUILayout.Popup("Edit language", editingLang, langCtrl.GetLangs());
        if (editingLang != newEditingLang)
        {
            langCtrl.EditLang(newEditingLang);
            changedEditingLang = true;
        }

        // display <key-phrase, value> pairs in dictionary of lang being edited
        dict = langCtrl.GetDictOfEditingLang();
        if (changedEditingLang || addedKey) // need to update editableDict to match new lang
        {
            editableDict.Clear();
            foreach (KeyValuePair<string, string> entry in dict)
            {
                editableDict.Add(entry.Key, entry.Value);
            }
            changedEditingLang = false;
            addedKey = false;

        }
        // display and update any values that we get
        foreach (KeyValuePair<string, string> entry in dict)
        {
            editableDict[entry.Key] = EditorGUILayout.TextField(entry.Key, editableDict[entry.Key]);
        }
        // allow user to save changes or remove the language being edited
        string currEditing = langCtrl.NameOfLangBeingEdited();
        if (GUILayout.Button("Save changes to '" + currEditing + "'"))
        {
            keyPhraseList = langCtrl.GetKeyPhrases();
            foreach (string keyPhrase in keyPhraseList)
            {
                Debug.Log("old val " + dict[keyPhrase] + " new val " + editableDict[keyPhrase]);
                if(dict[keyPhrase] != editableDict[keyPhrase])
                {
                    langCtrl.UpdateValue(currEditing, keyPhrase, editableDict[keyPhrase]);
                }
            }
        }
        if (GUILayout.Button("Remove language '" + currEditing + "'"))
        {
            langCtrl.RemoveLang(currEditing);
        }


    }
}
