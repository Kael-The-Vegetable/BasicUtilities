using BasicUtilities;
using UnityEditor;
using UnityEditor.UIElements;
using System.Globalization;
using System.Text;
using UnityEngine.UIElements;
#if UNITY_EDITOR
public class SerializableDictionaryConverter<KeyType, ValueType> : UxmlAttributeConverter<SerializableDictionary<KeyType, ValueType>>
{
    static string ValueToString(object val) => System.Convert.ToString(val, CultureInfo.InvariantCulture);

    public override string ToString(SerializableDictionary<KeyType, ValueType> source)
    {
        var builder = new StringBuilder();

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
    SerializedProperty dictionaryProperty;
    SerializedProperty keysProperty;
    SerializedProperty valuesProperty;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        dictionaryProperty = property;
        keysProperty = dictionaryProperty.FindPropertyRelative("SerializedKeys");
        valuesProperty = dictionaryProperty.FindPropertyRelative("SerializedValues");

        VisualElement container = new VisualElement();

        ListView ctx = new ListView()
        {
            showAddRemoveFooter = true,
            showAlternatingRowBackgrounds = AlternatingRowBackground.All,
            showBorder = true,
            showBoundCollectionSize = false,
            showFoldoutHeader = true,
            reorderable = false,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            headerTitle = property.displayName,
            bindingPath = property.propertyPath,
            bindItem = (element, index) =>
            {
                element.Clear();

                var elementKey = keysProperty.GetArrayElementAtIndex(index);
                var elementValue = valuesProperty.GetArrayElementAtIndex(index);

                element.Add(new PropertyField(elementKey));
                element.Add(new PropertyField(elementValue));

                element.Bind(dictionaryProperty.serializedObject);
            },
            overridingAddButtonBehavior = (listView, button) =>
            {
                keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
                dictionaryProperty.serializedObject.ApplyModifiedProperties();
            },
        };
        container.Add(ctx);

        return container;
    }
}
#endif // UNITY_EDITOR