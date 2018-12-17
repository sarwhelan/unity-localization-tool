using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogController))]
[ExecuteInEditMode]
public class DialogEditor : Editor {

    private string newTree;
    private int editingTree;
    private int newEditingTree;
    private bool needsRefresh = true;
    private List<DialogPromptNode> promptList = new List<DialogPromptNode>();
    private string newName;
    private string newNode;
    private string[] keyPhrases;
    private string[] promptIds;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // dialogCtrl enables us to call public methods of DialogController
        DialogController dialogCtrl = (DialogController)target;

        promptList = dialogCtrl.GetPromptsForTree();
        promptIds = dialogCtrl.GetPromptIds();
        keyPhrases = dialogCtrl.GetKeyPhrases();

        if (needsRefresh)
        {
            newName = dialogCtrl.NameOfEditingTree();
            editingTree = dialogCtrl.TreeBeingEditedIndex();
            promptList = dialogCtrl.GetPromptsForTree();
            promptIds = dialogCtrl.GetPromptIds();

            needsRefresh = false;
        }

        // create new dialog tree
        GUILayout.BeginHorizontal();
        newTree = EditorGUILayout.TextField("New dialog tree", newTree);
        if (GUILayout.Button("Add tree"))
        {
            dialogCtrl.AddNewTree(newTree);
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // display existing dialog trees to edit in a drop down menu,
        // aiming to have the same look & feel as the localization tool
        // to allow for consistency
        newEditingTree = EditorGUILayout.Popup("Edit dialog tree", editingTree, dialogCtrl.GetTrees());
        if (editingTree != newEditingTree)
        {
            dialogCtrl.SetEditTree(newEditingTree);
            needsRefresh = true;
        }

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        EditorGUILayout.LabelField("--- You're currently editing the dialog tree '" + dialogCtrl.NameOfEditingTree() + "' ---", style);
        style = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter };
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Remove dialog tree '" + dialogCtrl.NameOfEditingTree() + "'", style))
        {
            dialogCtrl.RemoveTree(dialogCtrl.NameOfEditingTree());
            needsRefresh = true;

        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space();

        // allow for renaming the dialog tree
        GUILayout.BeginHorizontal();
        newName = EditorGUILayout.TextField("Rename tree", newName);
        if (GUILayout.Button("Rename"))
        {
            if (dialogCtrl.NameOfEditingTree() != newName)
            {
                dialogCtrl.RenameEditingTree(newName);
                needsRefresh = true;
            }

        }
        GUILayout.EndHorizontal();

        // allow new node to be added to current tree being edited
        GUILayout.BeginHorizontal();
        newNode = EditorGUILayout.TextField("New node", newNode);
        if (GUILayout.Button("Add"))
        {
            dialogCtrl.AddNode(newNode);
            needsRefresh = true;
        }
        GUILayout.EndHorizontal();

        // display the DialogPromptNodes associated with the tree being edited (if changed -> reload)
        if (promptList != null)
        {
            foreach (DialogPromptNode prompt in promptList)
            {
                GUILayout.BeginVertical("Box");

                // prompt id and option to remove it
                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("-", GUILayout.Width(20f), GUILayout.Height(18f)))
                {
                    dialogCtrl.RemoveNode(prompt);
                    needsRefresh = true;
                    break;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Prompt node: " + prompt.GetNodeID(), EditorStyles.boldLabel);
                GUILayout.EndHorizontal();

                // option to pick associated keyphrase
                var oldPhraseIndex = keyPhraseIndex(prompt.GetKeyPhrase());
                var phraseIndex = EditorGUILayout.Popup("Key phrase", oldPhraseIndex, keyPhrases);
                if (oldPhraseIndex != phraseIndex)
                {
                    dialogCtrl.UpdatePromptPhrase(prompt, phraseIndex);
                }

                EditorGUILayout.Space();

                // responses title and add button
                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("+ new response", GUILayout.Width(100f), GUILayout.Height(18f)))
                {
                    dialogCtrl.AddPromptResponse(prompt);
                    needsRefresh = true;
                    break;
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.LabelField("Responses:", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();

                if (prompt.GetResponses() != null)
                {
                    foreach (DialogResponse resp in prompt.GetResponses())
                    {
                        //GUI.backgroundColor = Color.HSVToRGB(0f, 0f, 67.5f);
                        GUILayout.BeginVertical("Box");

                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("-", GUILayout.Width(20f), GUILayout.Height(18f)))
                        {
                            dialogCtrl.RemoveResponse(prompt, resp);
                            needsRefresh = true;
                            break;
                        }
                        GUI.backgroundColor = Color.white;

                        var oldResp = keyPhraseIndex(resp.GetKeyPhrase());
                        var newRespIndex = EditorGUILayout.Popup("Key phrase", oldResp, keyPhrases);
                        if (oldResp != newRespIndex)
                        {
                            dialogCtrl.UpdateRespPhrase(prompt, resp, newRespIndex);
                            needsRefresh = true;
                            break;
                        }

                        var oldGoTo = gotoPromptIndex(resp.GetNext());
                        var newGoToIndex = EditorGUILayout.Popup("Go to", oldGoTo, promptIds);
                        if (oldGoTo != newGoToIndex)
                        {
                            dialogCtrl.UpdateRespGoTo(prompt, resp, newGoToIndex);
                            needsRefresh = true;
                            break;
                        }
                        GUILayout.EndVertical();
                    }

                }
                GUILayout.EndVertical();
            }
        }




    }

    private int keyPhraseIndex(string keyPhrase)
    {
        int index = 0;
        foreach (string phrase in keyPhrases)
        {
            if (phrase == keyPhrase)
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    private int gotoPromptIndex(string next)
    {
        int index = 0;
        foreach (DialogPromptNode prompt in promptList)
        {
            if (next == prompt.GetNodeID())
            {
                return index;
            }
            index++;
        }
        return -1;
    }

}