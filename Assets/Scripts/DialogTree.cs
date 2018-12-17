using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogTree
{
    public string treeId;
    private List<DialogPromptNode> promptNodes = new List<DialogPromptNode>(); // actual prompts with all data associated with them
    private List<string> promptIds; // names of prompts

    public DialogTree(string id)
    {
        this.treeId = id;
        this.promptIds = new List<string>();
    }

    // AddPrompt adds a prompt with treeId 'id' to the collection of prompts for this tree
    public void AddPrompt(string id)
    {
        if (!promptIds.Contains(id)) 
        {
            Debug.Log("added prompt " + id + " to tree " + this.treeId);
            DialogPromptNode p = new DialogPromptNode(id);
            promptNodes.Add(p);
            promptIds.Add(id);
        }

    }

    // GetPrompts returns the DialogPromptNodes
    public List<DialogPromptNode> GetPrompts()
    {
        return this.promptNodes;
    }

    // GetPromptIds returns the prompt id's in a string[]
    public string[] GetPromptIds()
    {
        return this.promptIds.ToArray();
    }

    // Rename updates the treeId
    public void Rename(string newName)
    {
        Debug.Log("renamed " + this.treeId + " to " + newName);
        this.treeId = newName;
    }

    // RemovePrompt removes the DialogPromptNode from the collection of prompts
    public void RemovePrompt(DialogPromptNode promptNode)
    {
        if (promptIds.Contains(promptNode.GetNodeID()))
        {
            Debug.Log("removed prompt " + promptNode.GetNodeID() + " from " + this.treeId);
            promptIds.Remove(promptNode.GetNodeID());
            promptNodes.Remove(promptNode);
        }
    }

    // EditPromptPhrase updates the specified prompt key-phrase
    public void EditPromptPhrase(DialogPromptNode prompt, string phrase)
    {
        if (promptIds.Contains(prompt.GetNodeID()))
        {
            Debug.Log("editing prompt phrase from " + prompt.GetKeyPhrase() + " to " + phrase);
            prompt.SetDialogText(phrase);
        }
    }

    // AddPromptResponse adds a blank response to the prompt specified
    public void AddPromptResponse(DialogPromptNode prompt)
    {
        if (promptIds.Contains(prompt.GetNodeID()))
        {
            Debug.Log("adding a prompt response for prompt " + prompt.GetKeyPhrase());
            prompt.AddResponse("unset");
        }
    }

    // UpdatePromptResponse modifies the response key-phrase associated with the prompt
    public void UpdatePromptResponse(DialogPromptNode prompt, DialogResponse resp, string newPhrase)
    {
        if (promptIds.Contains(prompt.GetNodeID()))
        {

            Debug.Log("updating prompt response phrase from " + resp.GetKeyPhrase() + " to " + newPhrase);
            prompt.UpdateResponse(resp, newPhrase);
        }
    }

    // RemovePromptResponse removes the response from the speciifed prompt
    public void RemovePromptResponse(DialogPromptNode prompt, DialogResponse resp) 
    {
        if (promptIds.Contains(prompt.GetNodeID()))
        {
            Debug.Log("removing prompt response " + resp.GetKeyPhrase() + " from prompt " + prompt.GetKeyPhrase());
            prompt.RemoveResp(resp);
        }
    }

    // UpdatePromptRespGoTo updates the go-to value associated with the specified prompt's response value
    public void UpdatePromptRespGoTo(DialogPromptNode prompt, DialogResponse resp, string goToPrompt)
    {
        if (promptIds.Contains(prompt.GetNodeID()))
        {
            Debug.Log("updating prompt resp go to from " + resp.GetNext() + " to " + goToPrompt);
            prompt.SetRespNext(resp, goToPrompt);
        }
    }

}
