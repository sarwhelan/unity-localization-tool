using UnityEngine;

[System.Serializable]
public class DialogResponse
{
    [SerializeField]
    public string keyPhrase;

    [SerializeField]
    public string next;

    public DialogResponse(string keyPhrase)
    {
        // the key phrase chosen from the list of key phrases created using the localization tool
        this.keyPhrase = keyPhrase;

    }

    // SetNext sets the dialog prompt node that follows this response
    public void SetNext(string next)
    {
        Debug.Log("in response, setting resp from " + this.next + " to " + next);
        this.next = next;
    }

    public string GetKeyPhrase()
    {
        return keyPhrase;
    }

    public void SetKeyPhrase(string phrase)
    {
        this.keyPhrase = phrase;
    }

    public string GetNext()
    {
        return this.next;
    }
    
}
