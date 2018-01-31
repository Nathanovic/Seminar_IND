using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenInteractor : MonoBehaviour {

	public bool activateDialog;
	public int linkedDialog = 0;

	public bool requireItem;
	public string itemName;
	private Inventory inventoryScript;

	private PlayerNavigator playerNavigator;//used to confirm that we have moved to a new location
	public InteractionButton[] createdInteractions;

	[HideInInspector][SerializeField]public CanvasGroup myCVG; //set in editorScript

	void Awake(){
		DeactivateSelf ();
	}

	void Start () {
		playerNavigator = transform.parent.GetComponent<PlayerNavigator> ();
		if (requireItem) {
			inventoryScript = transform.root.GetComponentInChildren<Inventory> ();
		}
	}

	public bool CanEnter(){
		if (!requireItem) {
			return true;
		}
		else {
			if (inventoryScript.ItemInInventory (itemName)) {
				requireItem = false;
				return true;
			}
			else {
				return false;
			}
		}
	}

	public void NavigateToMe(){
		myCVG.alpha = 1f;
		transform.SetSiblingIndex (0);

		playerNavigator.onDestinationReached += DestinationReached;
		playerNavigator.onNavigateToNewLoc += TransitionToOtherBackground;

		ResponseManager.instance.onDialogStarted += DeactivateButtons;
		ResponseManager.instance.onDialogEnded += ActivateButtons;
	}

	public void DestinationReached(){
		playerNavigator.onDestinationReached -= DestinationReached;
		if (activateDialog) {
			activateDialog = false;//now we cant activate a dialog anymore :)
			ResponseManager.instance.ActivateDialog (linkedDialog);
		} else {
			ActivateButtons ();
		}
	}

	void ActivateButtons(){
		myCVG.interactable = true;
		myCVG.blocksRaycasts = true;
	}

	void DeactivateButtons(){
		myCVG.interactable = false;
		myCVG.blocksRaycasts = false;		
	}

	public void DeactivateSelf(){//called by editorscript
		myCVG.alpha = 0f;
		DeactivateButtons ();
	}

	void TransitionToOtherBackground(){
		playerNavigator.onNavigateToNewLoc -= TransitionToOtherBackground;
		DeactivateSelf ();
		StartCoroutine (FadeMeOut ());

		ResponseManager.instance.onDialogStarted -= DeactivateButtons;
		ResponseManager.instance.onDialogEnded -= ActivateButtons;
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
