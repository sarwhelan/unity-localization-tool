using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DialogDemo))]
[ExecuteInEditMode]
public class DialogDemoEditor : Editor {

    private int selectedTreeIndex;
    private int newTreeIndex;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // allows us to call methods from dialogDemo 
        DialogDemo dialogDemo = (DialogDemo)target;

        selectedTreeIndex = dialogDemo.GetCurrentTreeIndex();
        newTreeIndex = EditorGUILayout.Popup("Dialog tree selection", selectedTreeIndex, dialogDemo.TreeArray());
        if (selectedTreeIndex != newTreeIndex)
        {
            dialogDemo.SetTree(newTreeIndex);

        }

    }

}
