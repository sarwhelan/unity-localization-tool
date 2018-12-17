using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

// Since Unity doesn't do this for you ha ha here is a way to make it work
[System.Serializable]
[ExecuteInEditMode]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception("num keys != num values");

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
