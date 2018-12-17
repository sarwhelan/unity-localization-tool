using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DialogDemo is an example of utilizing both the localization and dialog tools 
// in order to display a set of converstion prompts and replies that can be specified by the user
public class DialogDemo : MonoBehaviour {

    public Text promptText;
    public ResponseButton respButton;
    public Transform canvas;

    private LanguageController langCtrl;
    private DialogController dialogCtrl;
    private string currentTreeName;
    private DialogTree currentTreeObj;
    private List<DialogTree> treeList = new List<DialogTree>();
    private string[] treeIds;
    private List<DialogPromptNode> prompts = new List<DialogPromptNode>();
    private int index = 0;
    private List<ResponseButton> buttons = new List<ResponseButton>();
    private float yPosition = 600f;

    // Start registers us to the DialogController and LanguageController, as well as
    // initializes our treeList and treeIds
    private void Start()
    {
        // if there's any remaining buttons, get rid of them!!
        foreach (ResponseButton button in GetComponentsInParent<ResponseButton>())
        {
            Destroy(button);
        }

        this.AssignControllers();

        dialogCtrl.RegisterTreeListener(this);

        treeList = dialogCtrl.GetActualTrees();
        treeIds = dialogCtrl.GetTrees();

        this.RefreshDisplay(null);
    }

    // OnDestroy must unregister us from any notification ports and destroy any remaining buttons
    private void OnDestroy()
    {
        dialogCtrl.UnregisterTreeListener(this);

        foreach (ResponseButton button in buttons)
        {
            DestroyImmediate(button);
        }
        buttons.Clear();
    }

    // AssignControllers loads the localization and dialog tools
    private void AssignControllers()
    {
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
        }

        dialogCtrl = Resources.Load("Dialog Tool") as DialogController;
        if (dialogCtrl == null)
        {
            dialogCtrl = BuildDialogController.BuildAsset();
        }
    }

    // RefreshDisplay prompts the prompt text and response buttons to be updated
    // depending on a supplied DialogPromptNode
    public void RefreshDisplay(DialogPromptNode nextPrompt)
    {
        this.AssignControllers();
        currentTreeObj = dialogCtrl.GetDemoTree();

        promptText.text = "";
        foreach (ResponseButton button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();

        if (index == 0)
        {
            Debug.Log("index is 0!");
            this.AssignControllers();
            prompts = dialogCtrl.GetDemoTreePrompts(currentTreeObj);
            this.ShowPromptAndResponses(prompts[index]);
        }
        else if (nextPrompt != null)
        {
            this.AssignControllers();
            this.ShowPromptAndResponses(nextPrompt);
        }

    }

    // ShowPromptAndResponses instantiates any buttons and sets up the UI
    private void ShowPromptAndResponses(DialogPromptNode prompt)
    {
        this.AssignControllers();
        promptText.text = langCtrl.GetValue(prompt.GetKeyPhrase());
        yPosition = 600f;
        Debug.Log("there are " + prompt.GetResponses().Count + " responses");
        foreach (DialogResponse resp in prompt.GetResponses())
        {
            ResponseButton button = Instantiate(respButton) as ResponseButton;
            button.Setup(this, resp, canvas, yPosition, langCtrl.GetValue(resp.GetKeyPhrase()));
            buttons.Add(button);
            yPosition -= 100f;
        }

        index++;
    }

    // ChoseResponse is the listener to the button.onClick event, that is called
    // when a response button is chosen. This facilitates the next step towards
    // refreshing the screen to show the updated prompt & new responses
    public void ChoseResponse(DialogResponse resp)
    {
        this.AssignControllers();
        currentTreeObj = dialogCtrl.GetDemoTree();
        prompts = dialogCtrl.GetDemoTreePrompts(currentTreeObj);

        if (!string.IsNullOrEmpty(resp.GetNext()))
        {
            string goTo = resp.GetNext();
            foreach (DialogPromptNode p in prompts)
            {
                if (p.GetNodeID() == goTo)
                {
                    RefreshDisplay(p);
                    break;
                }
            }
        }
        else
        {
            RefreshDisplay(null);
        }
    }

    // ------------------------ methods related to choosing/getting a tree --------------------------

    // OnTreeUpdate is called by the DialogController when a tree is added, removed, or edited
    public void OnTreeUpdate()
    {
        this.AssignControllers();
        treeList = dialogCtrl.GetActualTrees();
        treeIds = dialogCtrl.GetTrees();
    }

    // GetCurrentTreeIndex returns the index of the currently chosen tree within the treeList
    public int GetCurrentTreeIndex()
    {
        this.AssignControllers();
        treeIds = dialogCtrl.GetTrees();

        int i = 0;
        foreach (string treeId in treeIds)
        {
            if (treeId == currentTreeName)
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    // TreeArray returns a string[] of the tree identifiers
    public string[] TreeArray()
    {
        this.AssignControllers();
        treeIds = dialogCtrl.GetTrees();
        return treeIds;
    }

    // SetTree sets the currently chosen dialog tree
    public void SetTree(int index)
    {
        this.AssignControllers();
        treeIds = dialogCtrl.GetTrees();
        treeList = dialogCtrl.GetActualTrees();

        currentTreeName = treeIds[index];
        foreach (DialogTree tree in treeList)
        {
            if (tree.treeId == currentTreeName)
            {
                currentTreeObj = tree;
            }
        }

        this.AssignControllers();
        dialogCtrl.SetDemoTree(currentTreeObj);

        Debug.Log("current dialog tree is now " + currentTreeName);
    }

    // -------------------------------------------------------------------------------------------
}
