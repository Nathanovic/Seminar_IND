using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResponseAction), true)]
[CanEditMultipleObjects]
public class TextResponseEditor : Editor {

	public override void OnInspectorGUI () {
		serializedObject.Update ();

		SerializedProperty showChIDProp = serializedObject.FindProperty ("showCharacter");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PropertyField (showChIDProp);
		if (showChIDProp.boolValue) {
			EditorGUILayout.PropertyField (serializedObject.FindProperty("characterID"), GUIContent.none);			
		}
		EditorGUILayout.EndHorizontal ();

		SerializedProperty isFinalRespProp = serializedObject.FindProperty ("isFinalResponse");
		EditorGUILayout.PropertyField (isFinalRespProp);
		string dialogTextLabel = "Dialog text:";
		if (!isFinalRespProp.boolValue) {
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
		if(isFinalRespProp.boolValue) {
			SerializedProperty namesList = serializedObject.FindProperty ("eventNames");
			SerializedProperty paramsList = serializedObject.FindProperty("eventParams");
			ShowCombinedList(namesList, paramsList);
		}

		DrawPropertiesExcluding (serializedObject, "m_Script", "characterID", "dialogText", 
			"isFinalResponse", "showCharacter", "responseType", "nextResponse", 
			"nextPlayerChoiceStep", "eventNames", "eventParams");

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
