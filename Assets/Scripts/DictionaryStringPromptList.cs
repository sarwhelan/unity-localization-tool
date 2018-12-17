using UnityEngine;
using System.Collections;

// Serializable version of the <string, List<DialogPromptNode>> dictionary, using
// the public serializable DialogPromptNode class
[System.Serializable]
[ExecuteInEditMode]
public class DictionaryStringPromptList : SerializableDictionary<string, ArrayList> { }
