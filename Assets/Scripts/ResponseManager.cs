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

		EventManager.StartListening ("Activate Dialog", ActivateDialogWithEvent);
	}

	void ActivateDialogWithEvent(string dialogIndexParam){
		int dialogIndex = int.Parse (dialogIndexParam);
		ActivateDialog (dialogIndex);
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

		TryShowCharacter (responseHolder);

		responseHolder.RespondAction ();
		responseText.text = responseHolder.dialogText;
	}

	public void Continue(){//called when the continue button is pressed
		if (state == ResponseState.playerResponse) {//geef npc response
			if (awaitedResponse != null) {
				ShowDialogTextNPC (awaitedResponse);
				awaitedResponse = null;
			}
			else {
				EnableTextField (false);
				EndDialog ();
			}
		} else {//player text
			if (onNPCResponseClickedAway != null) {//used to activate the next text option for the player or the response from the npc
				responseText.text = "Choose your response...";
				onNPCResponseClickedAway ();
			} else {
				EndDialog ();
			}
		}
	}

	void TryShowCharacter(ResponseAction responseAction){
		if (responseAction.showCharacter) {
			characterScript.CharacterSpeaks (responseAction.characterID);
		}
		else {
			responseText.fontStyle = FontStyle.Italic;
			characterScript.DisableCharacter ();
		}
	}

	public void ActivateNextPlayerChoice(int nextPlayerChoiceStep){
		continueButton.SetActive (false);
		dialogs [currentDialogID].ActivateNextChoice (nextPlayerChoiceStep);
	}

	void EndDialog(){
		state = ResponseState.invisible;
		EnableTextField(false);
		onDialogEnded ();//cannot be null, since dialog always subcribes to this if the dialog starts
	}

	void EnableTextField(bool active = true){
		if (!active) {
			onTextFieldDisabled ();//disable characterText
		}else if(!textFieldActive && active){
			if(onDialogStarted != null)
				onDialogStarted ();
		}

		if (active) {
			continueButton.SetActive (true);		
		}

		textFieldActive = active;

		cvg.alpha = active ? 1f : 0f;
		cvg.interactable = active;
		cvg.blocksRaycasts = active;

		responseText.fontStyle = FontStyle.Normal;
	}
}

public enum ResponseState{
	invisible,
	playerResponse,
	npcResponse
}