using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// DialogController holds the data associated with the dialog trees, as well as 
// facilitates all of the methods related to adding, removing, and updating anything to do with the trees
[System.Serializable]
[CreateAssetMenu]
[ExecuteInEditMode]
public class DialogController : ScriptableObject
{
    // holds all the data !! does all the things!!

    private List<DialogTree> dialogTrees = new List<DialogTree>();
    private List<string> dialogTreeIds = new List<string>();
    private string[] dialogTreeArr;
    private string editingTreeName;
    private DialogTree treeObj;
    private List<DialogDemo> treeListener = new List<DialogDemo>();
    private DialogTree treeChosenForDemo;

    private static LanguageController langCtrl;
    private List<string> keyPhrases = new List<string>();

    // OnDestroy removes this obj as a key-phrase listener
    private void OnDestroy()
    {
        langCtrl.UnregisterKeyListener(this);
    }

    // OnEnable loads the localization tool and registers this obj as a key-phrase listener
    private void OnEnable()
    {
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
        }
        keyPhrases = langCtrl.GetKeyPhrases();

        langCtrl.RegisterKeyListener(this);
    }

    public void RegisterTreeListener(DialogDemo listener)
    {
        if (!treeListener.Contains(listener))
        {
            treeListener.Add(listener);
        }
    }

    public void UnregisterTreeListener(DialogDemo listener)
    {
        treeListener.Remove(listener);
    }

    // raiseTreeUpdate is called by this script when any modifications are made to the tree
    private void raiseTreeUpdate()
    {
        foreach (DialogDemo listener in treeListener)
        {
            listener.OnTreeUpdate();
        }
    }

    // OnKeyPhraseUpdate is called by the langCtrl when the set of key-phrases is updated
    public void OnKeyPhraseUpdate()
    {
        keyPhrases = langCtrl.GetKeyPhrases();
    }

    // AddNewTree adds a new DialogTree to dialogTrees list
    public void AddNewTree(string newTree)
    {
        if (!dialogTreeIds.Contains(newTree))
        {
            DialogTree tree = new DialogTree(newTree);
            dialogTrees.Add(tree);
            dialogTreeIds.Add(newTree);
            dialogTreeArr = dialogTreeIds.ToArray();
        }

        this.raiseTreeUpdate();

    }

    // GetDemoTree returns the tree obj chosen in the dialog demo
    public DialogTree GetDemoTree()
    {
        return treeChosenForDemo;
    }

    // SetDemoTree stores the tree obj chosen in the dialog demo
    public void SetDemoTree(DialogTree tree)
    {
        treeChosenForDemo = tree;
    }

    // GetDemoTreePrompts returns the DialogPromptNodes associated with the demo tree selected
    public List<DialogPromptNode> GetDemoTreePrompts(DialogTree demoTree)
    {
        foreach (DialogTree tree in dialogTrees)
        {
            if (demoTree.treeId == tree.treeId)
            {
                return tree.GetPrompts();
            }
        }
        return new List<DialogPromptNode>();
        
    }

    // GetTrees returns a string array of the tree names in the dialogTreeIds list
    public string[] GetTrees()
    {
        dialogTreeArr = dialogTreeIds.ToArray();
        return dialogTreeArr;
    }

    // GetActualTrees returns a list of the DialogTree objs
    public List<DialogTree> GetActualTrees()
    {
        return dialogTrees;
    }

    // SetEditTree takes an index as a parameter and sets that index as the current tree being edited
    public void SetEditTree(int index)
    {
        if (index >= 0)
        {
            editingTreeName = dialogTreeArr[index];
            Debug.Log("now editing tree " + editingTreeName);
            foreach (DialogTree t in dialogTrees)
            {
                if (t.treeId == editingTreeName)
                {
                    treeObj = t;
                    break;
                }
            }
        }

    }

    // TreeBeingEditedIndex returns the index of the tree being edited, or -1 if it is not found
    public int TreeBeingEditedIndex()
    {
        int index = 0;
        if (dialogTreeArr != null)
        {
            foreach (string tree in dialogTreeArr)
            {
                if (tree == editingTreeName)
                {
                    return index;
                }
                index++;
            }
        }

        return -1;
    }

    // RemoveTree removes DialogTree from the list of trees, treeIds, and refreshes the tree array
    public void RemoveTree(string tree)
    {
        dialogTrees.Remove(treeObj);
        dialogTreeIds.Remove(tree);
        dialogTreeArr = dialogTreeIds.ToArray();

        editingTreeName = "none selected";
        treeObj = null;

        this.raiseTreeUpdate();

    }

    // GetPromptsForTree returns the list of DialogPromptNodes associated with the dialog tree
    public List<DialogPromptNode> GetPromptsForTree()
    {
        if (dialogTreeIds.Contains(editingTreeName))
        {
            List<DialogPromptNode> prompts = treeObj.GetPrompts();
            return prompts;
        }

        return null;
       
    }

    // NameOfEditingTree returns the string identifier of the current dialog tree being edited
    public string NameOfEditingTree()
    {
        return editingTreeName;
    }

    // RenameEditingTree renames the current dialog tree being edited
    public void RenameEditingTree(string newName)
    {
        string oldName = treeObj.treeId;
        Debug.Log("renaming " + oldName + " to " + newName);

        // rename actual id associated with obj in dialogTrees list
        getTree(oldName).Rename(newName);

        // remove old treeId and add new treeId from id list, refresh array list of ids
        dialogTreeIds.Remove(oldName);
        dialogTreeIds.Add(newName);
        dialogTreeArr = dialogTreeIds.ToArray();

        // reset tree obj we're editing
        editingTreeName = newName;
        foreach (DialogTree t in dialogTrees)
        {
            if (t.treeId == editingTreeName)
            {
                treeObj = t;
                break;
            }
        }

        this.raiseTreeUpdate();

    }

    // getTree returns the DialogTree obj associated with the current tree being edited
    private DialogTree getTree(string treeName)
    {
        foreach (DialogTree t in dialogTrees)
        {
            if (t.treeId == treeName)
            {
                return t;
            }
        }
        return null;

    }

    // AddNode instantiates a new DialogPromptNode in the tree being edited
    public void AddNode(string nodeId)
    {
        this.getTree(editingTreeName).AddPrompt(nodeId);

        this.raiseTreeUpdate();
    }

    // GetKeyPhrases returns an array of key-phrases
    public string[] GetKeyPhrases()
    {
        return keyPhrases.ToArray();
    }

    // RemoveNode removes the specified DialogPromptNode from the current dialog tree's prompt list
    public void RemoveNode(DialogPromptNode promptNode)
    {
        this.getTree(editingTreeName).RemovePrompt(promptNode);

        this.raiseTreeUpdate();
    }

    // UpdatePromptPhrase updates the key-phrase associated with the specified prompt
    public void UpdatePromptPhrase(DialogPromptNode prompt, int index)
    {
        string newPhrase = this.GetKeyPhrases()[index];
        this.getTree(editingTreeName).EditPromptPhrase(prompt, newPhrase);

        this.raiseTreeUpdate();
    }

    // AddPromptResponse adds a new unset response to the set of responses associated with the prompt
    public void AddPromptResponse(DialogPromptNode prompt)
    {
        this.getTree(editingTreeName).AddPromptResponse(prompt);

        this.raiseTreeUpdate();
    }

    // UpdateRespPhrase updates the response key-phrase associated with the given prompt
    public void UpdateRespPhrase(DialogPromptNode prompt, DialogResponse resp, int index)
    {
        string newPhrase = this.GetKeyPhrases()[index];
        this.getTree(editingTreeName).UpdatePromptResponse(prompt, resp, newPhrase);

        this.raiseTreeUpdate();
    }

    // RemoveResponse removes the response from the list of DialogResponses held by the DialogPromptNode
    public void RemoveResponse(DialogPromptNode prompt, DialogResponse response)
    {
        this.getTree(editingTreeName).RemovePromptResponse(prompt, response);

        this.raiseTreeUpdate();
    }

    // UpdateRespGoTo updates the go-to value of the selected response associated with the prompt
    public void UpdateRespGoTo(DialogPromptNode prompt, DialogResponse resp, int index)
    {
        List<DialogPromptNode> l = this.getTree(editingTreeName).GetPrompts();
        string goTo = l[index].GetNodeID();
        this.getTree(editingTreeName).UpdatePromptRespGoTo(prompt, resp, goTo);

        this.raiseTreeUpdate();
    }

    // GetPromptIds returns the identifiers associated with the prompts of the tree being edited currently
    public string[] GetPromptIds()
    {
        if (this.getTree(editingTreeName) == null)
        {
            return new string[0];
        }
        else 
        {
            return this.getTree(editingTreeName).GetPromptIds();
        }
    }



}
