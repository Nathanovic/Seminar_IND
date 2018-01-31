using UnityEngine;
using UnityEditor;

public class EditorList : MonoBehaviour {

	public static void Show(SerializedProperty list, bool showListLabel = true){
		if (showListLabel) {
			EditorGUILayout.PropertyField (list);
			EditorGUI.indentLevel++;
		}

		if (list.isExpanded) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i));
			}
		}

		if (showListLabel) {
			EditorGUI.indentLevel--;
		}
	}
}
