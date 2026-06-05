using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BasicUtilities.Editor
{
	[CustomPropertyDrawer(typeof(FloatRange))]
	public class FloatRangeEditor : PropertyDrawer
	{
		public VisualTreeAsset vt;

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement root = new VisualElement();
			vt.CloneTree(root);
			root.Q<Label>("Name").text = property.displayName;
			root.Q<FloatField>("Min").BindProperty(property.FindPropertyRelative("_min"));
			root.Q<FloatField>("Max").BindProperty(property.FindPropertyRelative("_max"));
			return root;
		}
	}
}