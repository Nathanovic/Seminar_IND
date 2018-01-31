using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor {

	public override void OnInspectorGUI (){
		serializedObject.Update ();

		DrawPropertiesExcluding (serializedObject, "m_Script");

		if (GUILayout.Button ("Refresh ChoiceHolders")) {
			Dialog dialog = (Dialog)target;
			dialog.Insp_RefreshChoiceHolders ();			
		}

		serializedObject.ApplyModifiedProperties ();
	}
}
