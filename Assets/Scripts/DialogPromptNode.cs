using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogPromptNode
{
    [SerializeField]
    private string nodeId = "";

    [SerializeField]
    private string keyPhrase = "";

    [SerializeField]
    private List<DialogResponse> responses = new List<DialogResponse>();

    public DialogPromptNode(string nodeIdentifier)
    {
        this.nodeId = nodeIdentifier;
    }

    // SetDialogText sets the key-phrase of this prompt node
    public void SetDialogText(string keyPhrase)
    {
        this.keyPhrase = keyPhrase;
    }

    // AddResponse instantiates a new empty DialogResponse to be added to the responses list
    public void AddResponse(string keyPhrase)
    {
        DialogResponse response = new DialogResponse(keyPhrase);
        responses.Add(response);
    }

    // GetResponses returns the list of DialogResponses associated with this prompt
    public List<DialogResponse> GetResponses()
    {
        return responses;
    }

    // GetNodeID returns the identifier of this prompt node
    public string GetNodeID()
    {
        return this.nodeId;
    }

    // GetKeyPhrase returns the key-phrase associated with this prompt node
    public string GetKeyPhrase()
    {
        return this.keyPhrase;
    }

    // UpdateResponse modifies the specified response's key-phrase
    public void UpdateResponse(DialogResponse resp, string newPhrase)
    {
        resp.SetKeyPhrase(newPhrase);
    }

    // RemoveResp removes the specified response from the DialogPromptNode's list of responses
    public void RemoveResp(DialogResponse resp)
    {
        if (responses.Contains(resp)) 
        {
            responses.Remove(resp);
        }
    }

    // SetRespNext allows for specification of the next PromptNode associated with the specified response
    public void SetRespNext(DialogResponse resp, string nextPrompt)
    {
        if (responses.Contains(resp))
        {
            resp.SetNext(nextPrompt);
        }
    }


}
