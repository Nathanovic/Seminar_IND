using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResponseAction))]
[CanEditMultipleObjects]
public class TextResponseEditor : Editor {

	public override void OnInspectorGUI () {
		serializedObject.Update ();
		DrawPropertiesExcluding (serializedObject, "m_Script", "dialogText", "responseType", "nextResponse", "nextPlayerChoiceStep", "eventNames", "eventParams");

		bool isFinalResponse = serializedObject.FindProperty ("isFinalResponse").boolValue;
		string dialogTextLabel = "Dialog text:";
		if (!isFinalResponse) {
			SerializedProperty responseTypeProp = serializedObject.FindProperty ("responseType");
			EditorGUILayout.PropertyField (responseTypeProp);
			ResponseType respType = (ResponseType)responseTypeProp.enumValueIndex;
			if (respType == ResponseType.PlayerResponse) {
				SerializedProperty choiceStepProp = serializedObject.FindProperty ("nextPlayerChoiceStep");
				EditorGUILayout.PropertyField (choiceStepProp);
			}
			else {	
				SerializedProperty nextResponseProp = serializedObject.FindProperty ("nextResponse");
				EditorGUILayout.PropertyField (nextResponseProp);
			}
		}
		else {
			dialogTextLabel = "Event text:";
		}

		SerializedProperty dialogTextProp = serializedObject.FindProperty ("dialogText");
		EditorGUILayout.LabelField (dialogTextLabel);
		dialogTextProp.stringValue = EditorGUILayout.TextArea (dialogTextProp.stringValue);
		if(isFinalResponse) {
			SerializedProperty namesList = serializedObject.FindProperty ("eventNames");
			SerializedProperty paramsList = serializedObject.FindProperty("eventParams");
			ShowCombinedList(namesList, paramsList);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	void ShowCombinedList(SerializedProperty namesList, SerializedProperty paramsList){
		EditorGUILayout.PropertyField (namesList, new GUIContent("Events:"));
		EditorGUI.indentLevel++;

		if (namesList.isExpanded) {
			EditorGUILayout.PropertyField (namesList.FindPropertyRelative ("Array.size"));
			GUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Event:");
			EditorGUILayout.LabelField ("Parameter:");
			GUILayout.EndHorizontal ();
			paramsList.arraySize = namesList.arraySize;
			for (int i = 0; i < namesList.arraySize; i++) {
				SerializedProperty nameProp = namesList.GetArrayElementAtIndex (i);
				SerializedProperty paramProp = paramsList.GetArrayElementAtIndex (i);
				GUILayout.BeginHorizontal ();
				EditorGUILayout.PropertyField (nameProp, GUIContent.none);
				EditorGUILayout.PropertyField (paramProp, GUIContent.none);
				GUILayout.EndHorizontal ();
			}
		}

		EditorGUI.indentLevel--;
	}
}
