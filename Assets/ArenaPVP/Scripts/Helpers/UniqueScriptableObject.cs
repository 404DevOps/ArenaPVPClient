using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class UniqueScriptableObject : ScriptableObject
{

    [SerializeField]
    private string _uniqueID;

    public string Id => _uniqueID;

    private void OnValidate()
    {
        // Generate a unique ID only if it doesn't already exist
        if (string.IsNullOrEmpty(_uniqueID))
        {
            _uniqueID = Guid.NewGuid().ToString();
            Debug.Log($"Generated new ID for {name}: {_uniqueID}");
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets(); // Force save to prevent loss
#endif
        }
        else
        {
            // Check for duplicates (e.g., after duplication or serialization issues)
            ValidateUniqueID();
        }
    }

    private void ValidateUniqueID()
    {
        var allInstances = Resources.FindObjectsOfTypeAll<UniqueScriptableObject>();
        foreach (var instance in allInstances)
        {
            if (instance != this && instance._uniqueID == _uniqueID)
            {
                Debug.LogWarning($"Duplicate ID detected in {instance.name}. Generating a new one for {name}.");
                _uniqueID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this); // Mark object as dirty in editor
#endif
                break;
            }
        }
    }
}