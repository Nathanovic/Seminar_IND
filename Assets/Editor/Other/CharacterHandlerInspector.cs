using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

//this script enables the personhandler script to refresh the character data, both from the SO as from the hierarcy-panel
[CustomEditor(typeof(CharacterHandler))]
public class CharacterHandlerInspector : Editor {

	void OnEnable(){
		CharacterHandler script = (CharacterHandler)target;
		script.cvg.alpha = 1f;
	}

	public override void OnInspectorGUI (){
		base.OnInspectorGUI ();
		if (GUILayout.Button ("Refresh character data")) {
			string[] filePaths = Directory.GetFiles("Assets/GameData/Characters", "*.asset");
			List<Character> characters = new List<Character>(filePaths.Length);
			for(int i = 0; i < filePaths.Length; i ++){
				characters.Add (AssetDatabase.LoadAssetAtPath <Character> (filePaths [i]));
			}

			characters.Sort (SortByID);

			CharacterHandler characterScript = (CharacterHandler)target;
			characterScript.Insp_RefreshCharacters (characters.ToArray());
		}
	}

	static int SortByID(Character c1, Character c2){
		return c1.myID.CompareTo (c2.myID);
	}

	void OnDisable(){
		CharacterHandler script = (CharacterHandler)target;
		script.cvg.alpha = 0f;
	}
}
