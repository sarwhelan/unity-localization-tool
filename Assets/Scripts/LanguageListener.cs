using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LanguageListener : MonoBehaviour {

    public static LanguageController langCtrl;
    private Text thisText;
    private string currentKeyPhrase = "";

    // Start registers this listener to the language controller
	void Start () {
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
            Debug.Log("was null!");
        }
        langCtrl.RegisterLangListener(this);
        thisText = gameObject.GetComponent<Text>();
	}

    // OnDestroy unregisters this listener from the language controller
    private void OnDestroy()
    {
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
            Debug.Log("was null!");
        }
        langCtrl.UnregisterLangListener(this);
    }

    // OnLangChange is sort of an event raised by the LanguageEditor when a new language is selection
    // and will update the key-phrase chosen (THIS IS SEEN WHEN YOU FOCUS ON THE OBJ IN QUESTION BECAUSE
    // IT IS HAPPENING IN EDIT MODE)
    public void OnLangChange()
    {
        Debug.Log("in OnLangChange method");
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
            Debug.Log("was null!");
        }
        thisText.text = langCtrl.GetValue(currentKeyPhrase);
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    // SetKeyPhrase is called by the TextEditor when a key-phrase is chosen
    public void SetKeyPhrase(int keyPhraseIndex)
    {
        if (keyPhraseIndex >= 0)
        {
            string keyPhrase = GetKeyPhraseArray()[keyPhraseIndex];
            if (keyPhrase != currentKeyPhrase)
            {
                currentKeyPhrase = keyPhrase;
                langCtrl = Resources.Load("Localization Tool") as LanguageController;
                if (langCtrl == null)
                {
                    langCtrl = BuildLanguageController.BuildAsset();
                    Debug.Log("was null!");
                }
                thisText.text = langCtrl.GetValue(currentKeyPhrase);
                Debug.Log("updated key phrase");
            }
        }
    }

    // GetKeyPhraseArray returns the corresponding array to the language controller's
    // list of key-phrases
    public string[] GetKeyPhraseArray()
    {
        langCtrl = Resources.Load("Localization Tool") as LanguageController;
        if (langCtrl == null)
        {
            langCtrl = BuildLanguageController.BuildAsset();
            Debug.Log("was null!");
        }
        List<string> keyPhrases = langCtrl.GetKeyPhrases();
        return keyPhrases.ToArray();
    }

    // IndexOfCurrentKeyPhrase returns the index of the chosen key-phrase in the key-phrase array
    public int IndexOfCurrentKeyPhrase()
    {
        return getIndexOfKey(currentKeyPhrase);
    }

    // getIndexOfKey is a helper method to IndexOfCurrentKeyPhrase
    private int getIndexOfKey(string search)
    {
        string[] keyPhraseArray = GetKeyPhraseArray();
        int index = 0;
        foreach (string keyPhrase in keyPhraseArray)
        {
            if (keyPhrase == search)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
