using UnityEngine;

// Serializable version of the <string, Dictionary<string,string>> dictionary, using
// the created DictionaryStringString as the value! woo
[System.Serializable]
[ExecuteInEditMode]
public class DictionaryStringDictionary : SerializableDictionary<string, DictionaryStringString> {}
