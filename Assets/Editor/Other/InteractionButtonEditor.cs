using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractionButton))]
public class InteractionButtonEditor : Editor {

	void OnEnable(){
		InteractionButton script = (InteractionButton)target;
		script.Insp_Init ();
	}

	public override void OnInspectorGUI (){
		serializedObject.Update ();
		DrawPropertiesExcluding (serializedObject, "m_Script");
		serializedObject.ApplyModifiedProperties ();
	}
}
