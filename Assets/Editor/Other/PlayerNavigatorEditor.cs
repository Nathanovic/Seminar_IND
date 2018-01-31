using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerNavigator))]
public class PlayerNavigatorEditor : Editor {

	private PlayerNavigator script;

	void OnEnable(){
		script = (PlayerNavigator)target;
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		DrawPropertiesExcluding (serializedObject, "m_Script");

		if (GUILayout.Button ("Update Locations")) {
			script.allLocations = script.transform.GetComponentsInChildren<ScreenInteractor> ();
		}
		serializedObject.ApplyModifiedProperties ();
	}
}
