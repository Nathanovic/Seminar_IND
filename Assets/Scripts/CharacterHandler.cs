using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CharacterHandler : MonoBehaviour {

	public static CharacterHandler instance;

	public CanvasGroup cvg;
	[SerializeField]private Text characterText;
	[SerializeField]private Image characterHeadBG;

	[SerializeField]private Character[] allCharacters;
	[SerializeField]private Image[] characterImages;//only used to pass to Character
	public void Insp_RefreshCharacters(Character[] characters){
		allCharacters = characters;
	}

	private int[] activePersons;

	void Awake(){
		instance = this;
		ResponseManager.instance.onTextFieldDisabled += DeactivateCharacterText;
		ResponseManager.instance.onDialogEnded += DeactivateCharacters;
	}

	void Start(){
		DeactivateCharacters ();
		cvg.alpha = 1f;
	}

	public void ActivateCharacters(int[] personIDs){//on conversation starts
		activePersons = personIDs;
		for(int i = 0; i < activePersons.Length; i ++){//max 2
			allCharacters [personIDs[i]].Activate (
				characterImages[i]);
		}
	}

	public void CharacterSpeaks(int personID = -1){
		string characterName = (personID == -1) ? "You" : allCharacters [personID].myName;
		characterHeadBG.enabled = (personID == -1) ? false : true;
		characterText.enabled = true;
		characterText.text = characterName;
	}

	public void DisableCharacter(){
		DeactivateCharacterText ();
	}

	void DeactivateCharacterText(){
		characterText.enabled = false;
	}

	void DeactivateCharacters(){
		for (int i = 0; i < characterImages.Length; i++) {
			characterImages [i].enabled = false;
		}
	}
}
