using BasicUtilities;
using UnityEditor;
using UnityEditor.UIElements;
using System.Globalization;
using System.Text;
using UnityEngine.UIElements;
using System.Collections.Generic;
#if UNITY_EDITOR
public class SerializableDictionaryConverter<KeyType, ValueType> : UxmlAttributeConverter<SerializableDictionary<KeyType, ValueType>>
{
    static string ValueToString(object val) => System.Convert.ToString(val, CultureInfo.InvariantCulture);

    public override string ToString(SerializableDictionary<KeyType, ValueType> source)
    {
        StringBuilder builder = new();

        foreach (var kvp in source)
        {
            builder.Append($"{ValueToString(kvp.Key)}|{ValueToString(kvp.Value)},");
        }

        return builder.ToString();
    }

    public override SerializableDictionary<KeyType, ValueType> FromString(string value)
    {
        SerializableDictionary<KeyType, ValueType> dict = new();
        
        string[] KVPS = value.Split(',');
        foreach (string KVP in KVPS)
        {
            string[] fields = KVP.Split('|');
            KeyType key = (KeyType)System.Convert.ChangeType(fields[0], typeof(KeyType));
            ValueType val = (ValueType)System.Convert.ChangeType(fields[1], typeof(ValueType));
            dict.Editor_AddEntry(key, val);
        }
        dict.CorrectDictionary();
        return dict;
    }
}

[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    SerializedProperty dict;
    SerializedProperty keys;
    SerializedProperty values;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        dict = property;
        keys = property.FindPropertyRelative("SerializedKeys");
        values = property.FindPropertyRelative("SerializedValues");

        VisualElement container = new();

        ListView ctx = new()
        {
            showAddRemoveFooter = true,
            showAlternatingRowBackgrounds = AlternatingRowBackground.All,
            showBorder = true,
            showBoundCollectionSize = false,
            showFoldoutHeader = true,
            reorderable = false,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            headerTitle = property.displayName,
            bindingPath = keys.propertyPath,
            bindItem = BindListItem,
            overridingAddButtonBehavior = OnAddButton,
            onRemove = OnRemoveButton,
        };
        container.Add(ctx);

        Button removeDupes = new() 
        { 
            text = "Remove Duplicate Keys",
            tooltip = "Removes all duplicate keys from the dictionary, keeping only the first occurrence of each key.",
        };
        removeDupes.clicked += OnRemoveDuplicates;
        container.Add(removeDupes);

        return container;
    }

    #region Helper Methods
    private void BindListItem(VisualElement element, int index)
    {
        element.Clear();
        element.Unbind();

        var elementKey = keys.GetArrayElementAtIndex(index);
        var elementValue = values.GetArrayElementAtIndex(index);
        Label warning = new("<b>Error: Duplicate Key</b>");

        element.Add(new PropertyField(elementKey) { label = "Key" });
        element.Add(new PropertyField(elementValue) { label = "Value"});
        element.Add(warning);
        element.TrackPropertyValue(elementKey, (prop) => warning.visible = CheckForDuplicates(prop, index));
        warning.visible = CheckForDuplicates(elementKey, index);

        element.Bind(dict.serializedObject);
    }
    private bool CheckForDuplicates(SerializedProperty key, int index)
    {
        for (int i = 0; i < keys.arraySize; i++)
        {
            if (i == index) continue;
            if (CheckEqual(keys.GetArrayElementAtIndex(i), key)) return true;
        }
        return false;
    }
    private bool CheckEqual(SerializedProperty k1, SerializedProperty k2) => k1.boxedValue.Equals(k2.boxedValue);
    private void OnAddButton(BaseListView listView, Button button)
    {
        keys.InsertArrayElementAtIndex(keys.arraySize);
        values.InsertArrayElementAtIndex(values.arraySize);
        dict.serializedObject.ApplyModifiedProperties();
    }
    private void OnRemoveButton(BaseListView listView)
    {
        if (keys.arraySize == 0 || listView.selectedIndex < 0 || listView.selectedIndex >= keys.arraySize) return;
        int index = listView.selectedIndex;
        keys.DeleteArrayElementAtIndex(index);
        values.DeleteArrayElementAtIndex(index);
        dict.serializedObject.ApplyModifiedProperties();
    }
    private void OnRemoveDuplicates() 
    { 
        List<int> idsRemoved = new();
        for (int i = 0; i < keys.arraySize; i++)
        {
            if (idsRemoved.Contains(i)) continue;
            for (int j = i + 1; j < keys.arraySize; j++)
            {
                if (CheckEqual(keys.GetArrayElementAtIndex(i), keys.GetArrayElementAtIndex(j)))
                    idsRemoved.Add(j);
            }
        }

        for (int i = idsRemoved.Count - 1; i >= 0; i--)
        {
            int id = idsRemoved[i];
            keys.DeleteArrayElementAtIndex(id);
            values.DeleteArrayElementAtIndex(id);
        }
        dict.serializedObject.ApplyModifiedProperties();
    }
    #endregion
}
#endif // UNITY_EDITOR