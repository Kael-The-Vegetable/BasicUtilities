using System.Collections.Generic;
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

            SerializedKeys ??= new List<KeyType>();
            SerializedValues ??= new List<ValueType>();
            int count = Mathf.Min(SerializedKeys.Count, SerializedValues.Count);
            if (SerializedKeys.Count != SerializedValues.Count)
            { 
                Debug.LogWarning($"SerializedKeys and SerializedValues have different counts. Keys: {SerializedKeys.Count}, Values: {SerializedValues.Count}");
            }

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
            if (!this.ContainsKey(key))
            {
                this[key] = value;
                SerializedKeys.Add(key);
                SerializedValues.Add(value);
            }
            else
            {
                Debug.LogWarning($"Key {key} already exists in the dictionary.");
            }
        }
        public void Editor_RemoveEntry(KeyType key)
        {
            if (this.ContainsKey(key))
            {
                int index = SerializedKeys.IndexOf(key);
                if (index >= 0)
                {
                    SerializedKeys.RemoveAt(index);
                    SerializedValues.RemoveAt(index);
                }
                this.Remove(key);
            }
            else
            {
                Debug.LogWarning($"Key {key} does not exist in the dictionary.");
            }
        }
#endif // UNITY_EDITOR
        #endregion
    }
}
