using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ChoiceOption))]
public class ChoiceOptionDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		//int startIndentLevel = EditorGUI.indentLevel;

		SerializedProperty letterProp = property.FindPropertyRelative ("optionLetter");
		SerializedProperty removeProp = property.FindPropertyRelative ("removeOption");
		SerializedProperty responseProp = property.FindPropertyRelative ("responseAction");

		SerializedProperty buttonTextProp = property.FindPropertyRelative ("buttonText");
		SerializedProperty showDialogTextProp = property.FindPropertyRelative ("longerDialog");//bool
		SerializedProperty dialogTextProp = property.FindPropertyRelative ("dialogText");

		label = EditorGUI.BeginProperty (position, label, property);
		Rect contentPosition = EditorGUI.PrefixLabel (position, label);
		float totalWidth = contentPosition.width;
		float startX = contentPosition.x;
		contentPosition.height = 16f;

		//Option Letter:
		contentPosition.x = startX;
		contentPosition.width = 18f;
		EditorGUI.LabelField (contentPosition, letterProp.stringValue + ":", GUIStyle.none);

		//Button text:
		contentPosition.x = startX + 17f;
		contentPosition.width = totalWidth - 35f;
		EditorGUI.PropertyField (contentPosition, buttonTextProp, GUIContent.none); 

		//Dialog text checkbox:
		contentPosition.x = startX + totalWidth - 29f;
		EditorGUI.PropertyField (contentPosition, showDialogTextProp, GUIContent.none);

		contentPosition.x = startX + 17f;
		contentPosition.y += 18f;

		if (showDialogTextProp.boolValue) {
			contentPosition.height = 31f;
			dialogTextProp.stringValue = EditorGUI.TextArea (contentPosition, dialogTextProp.stringValue); 
			contentPosition.height = 16f;
			contentPosition.y += 34f;
		}

		contentPosition.width = totalWidth - 39f;
		EditorGUI.PropertyField (contentPosition, responseProp, GUIContent.none);

		contentPosition.x = startX + totalWidth - 18f;
		contentPosition.width = 18f;
		if (GUI.Button (contentPosition, "X")) {
			removeProp.boolValue = true;
		}

		EditorGUI.EndProperty ();
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label){
		if (property.name == "optionText")
			return 34f;
		else
			return 16f;
	}
}