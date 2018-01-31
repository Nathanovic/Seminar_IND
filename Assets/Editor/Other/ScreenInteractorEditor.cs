using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ScreenInteractor))]
public class ScreenInteractorEditor : Editor {

	ScreenInteractor script;
	Dictionary<InteractionButton, Editor> editors;

	void OnEnable(){
		script = (ScreenInteractor)target;
		editors = new Dictionary<InteractionButton, Editor> (4);
		script.myCVG = script.GetComponent<CanvasGroup> ();
		if (!Application.isPlaying) {
			script.myCVG.alpha = 1f;
			script.myCVG.interactable = true;
		}
	}
	
	public override void OnInspectorGUI (){
		int beforeCount = script.createdInteractions.Length;

		serializedObject.Update ();

		SerializedProperty activateDialogProp = serializedObject.FindProperty ("activateDialog");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PropertyField(activateDialogProp, new GUIContent("Activate dialog:"));
		if (activateDialogProp.boolValue == true) {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("linkedDialog"), GUIContent.none);			
		}
		EditorGUILayout.EndHorizontal ();

		DrawPropertiesExcluding (serializedObject, "m_Script", "activateDialog", "linkedDialog");
		serializedObject.ApplyModifiedProperties ();

		int afterCount = script.createdInteractions.Length;
		if (beforeCount != afterCount)
			return;

		for (int i = 0; i < script.createdInteractions.Length; i++) {
			InteractionButton interaction = script.createdInteractions [i];
			if (interaction == null) {
				EditorGUILayout.LabelField ("Null...");
			} else {
				EditorGUILayout.LabelField (interaction.name);
				EditorGUI.indentLevel++;
				FindEditorForInteraction (interaction).OnInspectorGUI ();
				EditorGUI.indentLevel--;
			}
		}

		if(GUILayout.Button("Refresh Interactions")){
			script.createdInteractions = script.transform.GetComponentsInChildren<InteractionButton> ();
			serializedObject.ApplyModifiedProperties ();
		}
	} 

	Editor FindEditorForInteraction(InteractionButton interaction){
		Editor currentEditor = null;
		if (!editors.TryGetValue (interaction, out currentEditor)) {
			currentEditor = CreateEditor (interaction);
			editors.Add (interaction, currentEditor);
		}
		return currentEditor;
	}

	void OnDisable(){
		if (!Application.isPlaying) {
			script.DeactivateSelf ();
		}
	}
}
