using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace BasicUtilities
{
    [System.Serializable]
    public class SerializableDictionary<KeyType, ValueType> : Dictionary<KeyType, ValueType>, ISerializationCallbackReceiver
    {
        public List<KeyType> SerializedKeys = new();
        public List<ValueType> SerializedValues = new();

        #region Serialization Callbacks
        public void OnAfterDeserialize()
        {
            CorrectDictionary();
        }

        public void OnBeforeSerialize() { }
        #endregion

        #region Helper Methods
        public void CorrectDictionary()
        {
            this.Clear();

            if (SerializedKeys == null || SerializedValues == null)
            {
                Debug.LogWarning("SerializedKeys or SerializedValues is null. Rebuilding...");
                SerializedKeys = new List<KeyType>();
                SerializedValues = new List<ValueType>();
            }
            
            if (SerializedKeys.Count != SerializedValues.Count)
            { 
                Debug.LogWarning($"SerializedKeys and SerializedValues have different counts. Keys: {SerializedKeys.Count}, Values: {SerializedValues.Count}. Rebuilding...");
                SerializedKeys = new(Keys);
                SerializedValues = new(Values);
            }

            int count = Mathf.Min(SerializedKeys.Count, SerializedValues.Count);
            for (int i = 0; i < count; i++)
            {
                this[SerializedKeys[i]] = SerializedValues[i];
            }
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        public void Editor_AddEntry(KeyType key, ValueType value)
        {
            SerializedKeys.Add(key);
            SerializedValues.Add(value);
        }
        public void Editor_RemoveEntry(KeyType key)
        {
            int id = SerializedKeys.IndexOf(key);
            SerializedKeys.RemoveAt(id);
            SerializedValues.RemoveAt(id);
        }
#endif // UNITY_EDITOR
        #endregion
    }
}
