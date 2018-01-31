using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResponseManager : MonoBehaviour {
	
	public static ResponseManager instance;
	private CanvasGroup cvg;
	[SerializeField]private Text responseText;
	[SerializeField]private GameObject continueButton;
	private ResponseAction awaitedResponse;

	public delegate void ResponseDelegate();
	public event ResponseDelegate onNPCResponseClickedAway;
	public event ResponseDelegate onDialogEnded;
	public event ResponseDelegate onTextFieldDisabled;
	public event ResponseDelegate onDialogStarted;

	[SerializeField]private ResponseState state;

	private bool textFieldActive;

	private int currentDialogID;//fill it in for testing the dialog
	[SerializeField]private Dialog[] dialogs;//given by DialogEditor
	public void Insp_RefreshDialogs(){//this should be called after a dialog has been added or removed
		dialogs = transform.parent.GetComponentsInChildren<Dialog>();
	}

	[SerializeField]private CharacterHandler characterScript;

	void Awake(){
		instance = this;
	}

	void Start(){
		cvg = GetComponent<CanvasGroup> ();
		characterScript = transform.parent.GetComponentInChildren<CharacterHandler> ();
		EnableTextField (false);
	}

	public void ActivateDialog(int dialogIndex){
		currentDialogID = dialogIndex;
		dialogs [dialogIndex].Activate ();
	}

	public void ShowDialogTextPlayer(ResponseAction responseHolder, string chosenText){
		state = ResponseState.playerResponse;
		EnableTextField ();
		characterScript.CharacterSpeaks ();

		awaitedResponse = responseHolder;
		responseText.text = chosenText;
	}

	public void ShowDialogTextNPC(ResponseAction responseHolder){
		state = ResponseState.npcResponse;
		EnableTextField ();
		characterScript.CharacterSpeaks (responseHolder.characterID);

		responseHolder.RespondAction ();
		responseText.text = responseHolder.dialogText;
	}

	public void Continue(){//called when the continue button is pressed
		if (state == ResponseState.playerResponse) {//geef npc response
			if (awaitedResponse != null) {
				state = ResponseState.npcResponse;

				responseText.text = awaitedResponse.dialogText;
				awaitedResponse.RespondAction ();
				if (awaitedResponse.showCharacter) {
					characterScript.CharacterSpeaks (awaitedResponse.characterID);
				}
				else {
					characterScript.DisableCharacter ();
				}
				awaitedResponse = null;
			}
			else {
				EnableTextField (false);
				EndDialog ();
			}
		} else {//player text
			EnableTextField (false);
			if (onNPCResponseClickedAway != null) {//used to activate the next text option for the player
				onNPCResponseClickedAway ();
			} else {
				EndDialog ();
			}
		}
	}

	public void ActivateNextPlayerChoice(int nextPlayerChoiceStep){
		dialogs [currentDialogID].ActivateNextChoice (nextPlayerChoiceStep);
	}

	void EndDialog(){
		state = ResponseState.invisible;
		onDialogEnded ();//cannot be null, since dialog always subcribes to this if the dialog starts
	}

	void EnableTextField(bool active = true){
		if (!active) {
			onTextFieldDisabled ();//disable characterText
		}else if(!textFieldActive && active){
			if(onDialogStarted != null)
				onDialogStarted ();
		}

		textFieldActive = active;

		cvg.alpha = active ? 1f : 0f;
		cvg.interactable = active;
		cvg.blocksRaycasts = active;
		continueButton.SetActive(active);
	}
}

public enum ResponseState{
	invisible,
	playerResponse,
	npcResponse
}