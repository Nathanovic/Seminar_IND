using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenInteractor : MonoBehaviour {

	public bool activateDialog;
	public int linkedDialog = 0;

	private PlayerNavigator playerNavigator;//used to confirm that we have moved to a new location
	public InteractionButton[] createdInteractions;

	[HideInInspector][SerializeField]public CanvasGroup myCVG; //set in editorScript

	void Awake(){
		DeactivateSelf ();
	}

	void Start () {
		playerNavigator = transform.parent.GetComponent<PlayerNavigator> ();
	}

	public void NavigateToMe(){
		myCVG.alpha = 1f;
		transform.SetSiblingIndex (0);

		playerNavigator.onDestinationReached += DestinationReached;
		playerNavigator.onNavigateToNewLoc += TransitionToOtherBackground;
	}

	public void DestinationReached(){
		playerNavigator.onDestinationReached -= DestinationReached;
		if (activateDialog) {
			activateDialog = false;//now we cant activate a dialog anymore :)
			ResponseManager.instance.onDialogEnded += DialogEnded;
			ResponseManager.instance.ActivateDialog (linkedDialog);
		} else {
			ActivateButtons ();
		}
	}

	void DialogEnded(){
		ResponseManager.instance.onDialogEnded -= DialogEnded;		
		ActivateButtons ();
	}

	void ActivateButtons(){
		myCVG.interactable = true;
		myCVG.blocksRaycasts = true;
	}

	void DeactivateButtons(){//called when the textfield is shown
		//ResponseManager.instance.onTextFieldDisabled += ActivateButtons;
	}

	public void DeactivateSelf(){//called by editorscript
		myCVG.alpha = 0f;
		myCVG.interactable = false;
		myCVG.blocksRaycasts = false;
	}

	void TransitionToOtherBackground(){
		playerNavigator.onNavigateToNewLoc -= TransitionToOtherBackground;
		DeactivateSelf ();
		StartCoroutine (FadeMeOut ());
	}

	IEnumerator FadeMeOut(){
		float t = 0f;
		while (t < 1f) {
			t += Time.deltaTime;
			myCVG.alpha = 1 - t;
			yield return null;
		}

		playerNavigator.DestinationReached ();
	}
}
