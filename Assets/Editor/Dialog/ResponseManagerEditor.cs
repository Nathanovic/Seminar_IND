using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResponseManager))]
public class ResponseManagerEditor : Editor {

	public override void OnInspectorGUI (){
		serializedObject.Update ();

		DrawPropertiesExcluding (serializedObject, "m_Script");

		if (GUILayout.Button ("Refresh dialogs")) {
			ResponseManager responseManager = (ResponseManager)target;
			responseManager.Insp_RefreshDialogs ();			
		}

		serializedObject.ApplyModifiedProperties ();
	}
}
