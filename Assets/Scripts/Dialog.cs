using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dit script is er om een gesprek te activeren en te reguleren
//activeren: door de gamemanager
//reguleren: hier zijn alle choiceholders en vanaf hier alleen wordt een choiceholder geactivated
public class Dialog : MonoBehaviour {

	[HideInInspector][SerializeField]private CharacterHandler characterScript;

	private int currentChoiceHolderID;
	[SerializeField]private ChoiceHolder[] choiceHolders;//given by dialogEditor
	public void Insp_RefreshChoiceHolders(){
		choiceHolders = GetComponentsInChildren<ChoiceHolder> ();
	}
	public ResponseAction npcDialogStarter;//can be null, if we start with a choiceHolder
	public bool dialogEnded;

	public int[] npcCharacterIDs = new int[2];//used by dialog, to prepare CharacterHandler

	void Start(){
		characterScript = transform.parent.GetComponentInChildren<CharacterHandler> ();
	}

	public void ActivateNextChoice(int nextChoiceStep){//activate the next text option
		currentChoiceHolderID += nextChoiceStep;
		choiceHolders [currentChoiceHolderID].ShowOptions ();
	}

	public void Activate(){
		characterScript.ActivateCharacters (npcCharacterIDs); 

		if (!npcDialogStarter) {
			choiceHolders [0].ShowOptions ();
		}
		else {
			ResponseManager.instance.ShowDialogTextNPC (npcDialogStarter);
		}
			
		ResponseManager.instance.onDialogEnded += DeactivateDialog;
	}

	public void DeactivateDialog(){
		ResponseManager.instance.onDialogEnded -= DeactivateDialog;
		dialogEnded = true;
	}
}