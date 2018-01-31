using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChoiceHolder))]
[CanEditMultipleObjects]
public class ChoiceHolderEditor : Editor {

	private ChoiceHolder choiceHolder;
	private int prevChoiceCount;
	private List<ChoiceOption> createdOptions = new List<ChoiceOption> ();

	void OnEnable(){
		choiceHolder = (ChoiceHolder)target;
		choiceHolder.Insp_Enabled ();

		prevChoiceCount = choiceHolder.createdOptions.Count;
		createdOptions.Clear ();
		for (int i = 0; i < prevChoiceCount; i++) {
			ChoiceOption editorOption = EditorOptionFromOption (choiceHolder.createdOptions [i]);
			createdOptions.Add (editorOption);
		}
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();

		DrawPropertiesExcluding (serializedObject, "m_Script", "createdOptions", "optionPrefab");
		ShowList(serializedObject.FindProperty("createdOptions"));

		int choiceCount = choiceHolder.createdOptions.Count;

		if (prevChoiceCount != choiceCount) {
			if (choiceCount > prevChoiceCount) {
				for (int i = prevChoiceCount; i < choiceCount; i++) {
					ChoiceOption newOption = null; 
					choiceHolder.Insp_CreateVisibleOption (i, out newOption);
					createdOptions.Add (EditorOptionFromOption (newOption));
				}
			} else {
				Debug.LogWarning ("Corrupted choice holder");
			}

			prevChoiceCount = choiceCount;
		}

		CheckForChangedOptions ();

		serializedObject.ApplyModifiedProperties ();
	}

	void ShowList(SerializedProperty list){
		EditorGUILayout.PropertyField (list);
		EditorGUI.indentLevel++;

		if (list.isExpanded) {
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				SerializedProperty listProp = list.GetArrayElementAtIndex (i);
				float propertyHeight = 34f;
				if (listProp.FindPropertyRelative ("longerDialog").boolValue == true) {
					propertyHeight = 68f;
				}
				EditorGUILayout.PropertyField (listProp, GUIContent.none, GUILayout.Height(propertyHeight));//48
			}
		}

		EditorGUI.indentLevel--;
	}

	void CheckForChangedOptions(){
		for (int i = 0; i < choiceHolder.createdOptions.Count; i++) {
			ChoiceOption inspectedOption = choiceHolder.createdOptions [i];
			if (inspectedOption.removeOption) {
				choiceHolder.Insp_RemoveOption (i);
				createdOptions.RemoveAt (i);
				prevChoiceCount--;

				Debug.Log ("destroyed option, repainting scene");
				SceneView.RepaintAll ();
			}
			else if (inspectedOption.buttonText != createdOptions [i].buttonText || inspectedOption.dialogText != createdOptions [i].dialogText) {
				createdOptions [i].buttonText = inspectedOption.buttonText;
				createdOptions [i].dialogText = inspectedOption.dialogText;
				choiceHolder.createdOptions [i].UpdateText ();
			}else if(inspectedOption.longerDialog != createdOptions[i].longerDialog){
				createdOptions [i].longerDialog = inspectedOption.longerDialog;
				choiceHolder.createdOptions [i].UpdateText ();
			}
			else if (inspectedOption.responseAction != createdOptions [i].responseAction) {
				createdOptions [i].responseAction = inspectedOption.responseAction;
				inspectedOption.UpdateResponseAction ();
			}
		}
	}

	//all that the editor knows about the options is the text, the responseAction and the amount of options
	//this function is useful when a new option is created and for initialization OnEnable
	ChoiceOption EditorOptionFromOption(ChoiceOption option){
		ChoiceOption newEditorOption = new ChoiceOption ();
		newEditorOption.buttonText = option.buttonText;
		newEditorOption.dialogText = option.dialogText;
		newEditorOption.longerDialog = option.longerDialog;
		newEditorOption.responseAction = option.responseAction;
		return newEditorOption;
	}

	void OnDisable(){
		choiceHolder.Insp_Disabled ();
	}
}