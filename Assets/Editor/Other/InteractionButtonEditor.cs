using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractionButton))]
[CanEditMultipleObjects]
public class InteractionButtonEditor : Editor {

	InteractionButton script;

	void OnEnable(){
		script = (InteractionButton)target;
		if (script == null)
			return;
		script.Insp_Init ();
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		DrawPropertiesExcluding (serializedObject, "m_Script");
		serializedObject.ApplyModifiedProperties ();
	}

	void OnDisable(){
		if (!Application.isPlaying && script != null) {
			VisibleBackground.TryChangingBackground ();
		}		
	}
}
