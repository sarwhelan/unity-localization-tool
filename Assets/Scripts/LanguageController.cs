using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu]
[ExecuteInEditMode]
public class LanguageController : ScriptableObject
{
    private string currentLang = "";
    private DictionaryStringDictionary langCollection = new DictionaryStringDictionary();
    private List<string> keyPhrases = new List<string>();
    private List<string> langs = new List<string>();
    private string[] langArr;
    private string langBeingEdited = "none selected";
    private List<LanguageListener> langListeners = new List<LanguageListener>();
    private List<DialogController> keyListeners = new List<DialogController>();
    //private List<DialogDemo> dialogLangListeners = new List<DialogDemo>();

    // raiseLangChange notifies all language listeners of a language change
    private void raiseLangChange()
    {
        foreach(LanguageListener listener in langListeners)
        {
            listener.OnLangChange();
        }

        //foreach(DialogDemo listener in dialogLangListeners)
        //{
        //    listener.OnLangChange();
        //}
    }

    private void raiseKeyUpdate()
    {
        foreach(DialogController listener in keyListeners)
        {
            listener.OnKeyPhraseUpdate();
        }
    }

    //// RegisterDialogLangListener adds the listener to the listeners list
    //public void RegisterDialogLangListener(DialogDemo listener)
    //{
    //    if (!dialogLangListeners.Contains(listener))
    //    {
    //        dialogLangListeners.Add(listener);
    //        Debug.Log("just added dialog lang listener");
    //    }
    //}

    //// UnregisterDialogLangListener removes the listener from the listeners list
    //public void UnregisterDialogLangListener(DialogDemo listener)
    //{
    //    dialogLangListeners.Remove(listener);
    //}

    // RegisterListener adds the listener to the listeners list
    public void RegisterLangListener(LanguageListener listener)
    {
        if (!langListeners.Contains(listener))
        {
            langListeners.Add(listener);
            Debug.Log("just added regular ol' lang listener");
        }
    }

    // UnregisterListener removes the listenenr from the listeners list
    public void UnregisterLangListener(LanguageListener listener)
    {
        langListeners.Remove(listener);
    }

    public void RegisterKeyListener(DialogController listener)
    {
        if (!keyListeners.Contains(listener))
        {
            keyListeners.Add(listener);
            Debug.Log("just added key listener");
        }
    }

    public void UnregisterKeyListener(DialogController listener)
    {
        keyListeners.Remove(listener);
    }

    // AddNewLang adds a new <language, dictionary<key-phrase, value>> to the langCollection,
    // and initializes the <key-phrase,value> dictionary with the key-set of the other languages and an empty string value,
    // else if there are no other languages, initializes an empty dictionary
    public void AddNewLang(string newLang) 
    {
        if (!langs.Contains(newLang))
        {
            langs.Add(newLang);
            DictionaryStringString dict = new DictionaryStringString();
            foreach (string key in keyPhrases)
            {
                dict.Add(key, "");
            }
            langCollection.Add(newLang, dict);
            Debug.Log("added language " + newLang);
            Debug.Log("now list of languages is");
            foreach(string lang in langCollection.Keys)
            {
                Debug.Log(lang);
            }
        }
    }

    // RemoveLang removes the specified language from the langCollection
    public void RemoveLang(string lang) 
    {
        if (langCollection.ContainsKey(lang))
        {
            langCollection.Remove(lang);
            langs.Remove(lang);
            langArr = langs.ToArray();

            if (langBeingEdited == lang)
            {
                langBeingEdited = "none selected";
            }
            Debug.Log("removed language " + lang);
        }
        else
        {
            Debug.Log("can't remove/doesn't contain " + lang);
        }
    }

    // AddNewKey iterates through the languages and adds the specified key
    // to each languages <key-phrase,value> dictionary
    public void AddNewKey(string key) 
    {
        if (!keyPhrases.Contains(key))
        {
            keyPhrases.Add(key);
            foreach (string lang in langs)
            {
                Debug.Log("adding key " + key + " to lang " + lang);
                DictionaryStringString dict = langCollection[lang];
                dict.Add(key, "");
                langCollection[lang] = dict;
            }
            Debug.Log("added new key: " + key);
            raiseKeyUpdate();
        }
        else
        {
            Debug.Log("key already exists: " + key);
        }
    }

    // UpdateValue updates the value associated with a specific key-phrase in a specified language
    public void UpdateValue(string language, string key, string value) 
    {
        DictionaryStringString langDict = langCollection[language];
        langDict[key] = value;
        langCollection[language] = langDict;
        Debug.Log("updated keyPhrase " + key + " to value " + value + " in lang " + language);
    }

    // SelectLang is called when the language selection popup (I really think this should
    // be called dropdown but!!??) has an item selected
    public void SelectLang(int langIndex) 
    {
        if (langIndex >= 0)
        {
            string lang = langArr[langIndex];
            if (currentLang != lang) // if we are in fact changing the language
            {
                currentLang = lang;
                raiseLangChange();
            }
        }
    }

    // EditLang is called when the edit language popup has an item selected
    public void EditLang(int langIndex)
    {
        if (langIndex >= 0)
        {
            langBeingEdited = langArr[langIndex];
        }
    }

    // GetLangs returns a string array of languages instantiated for the popup GUI element
    public string[] GetLangs()
    {
        langArr = langs.ToArray();
        return langArr;
    }

    // IndexOfCurrentLang returns the index of the current language to be displayed from the langArr
    public int IndexOfCurrentLang()
    {
        int index = 0;
        if (langArr != null)
        {
            foreach (string lang in langArr)
            {
                if (lang == currentLang)
                {
                    return index;
                }
                index++;
            }
        }

        return -1;
    }

    // IndexOfLangBeingEdited returns the index of the current language being edited
    public int IndexOfLangBeingEdited()
    {
        int index = 0;
        if (langArr != null)
        {
            foreach (string lang in langArr)
            {
                if (lang == langBeingEdited)
                {
                    return index;
                }
                index++;
            }
        }

        return -1;
    }

    // GetDictOfEditingLang returns the dictionary associated with the language being edited
    // in order to display the set of key-phrase, value pairs
    public DictionaryStringString GetDictOfEditingLang()
    {
        if(langCollection.ContainsKey(langBeingEdited))
        {
            return langCollection[langBeingEdited];
        }
        return new DictionaryStringString();
    }

    // GetKeyPhrases returns the language controller's list of key-phrases
    public List<string> GetKeyPhrases()
    {
        return keyPhrases;
    }

    // NameOfLangBeingEdited returns the name of the language selected for editing
    public string NameOfLangBeingEdited() 
    {
        return langBeingEdited;
    }

    // GetValue returns the value associated with the key-phrase provided in the 
    // dictionary for the currently selected language
    public string GetValue(string keyPhrase)
    {
        if (langCollection.ContainsKey(currentLang))
        {
            DictionaryStringString dict = langCollection[currentLang];
            if (dict.ContainsKey(keyPhrase))
            {
                return dict[keyPhrase];
            }

        }
        return "";
    }



}
