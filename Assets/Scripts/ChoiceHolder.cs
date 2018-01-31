using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//deze class heeft alle options en kan makkelijk de tekst ervan aanpassen
public class ChoiceHolder : MonoBehaviour {

	[SerializeField]private GameObject optionPrefab;
	[SerializeField]private Vector3 nextOptionPos = new Vector3(-300,-450,0);
	[SerializeField]private Vector3 optionOffset = new Vector3(600,0,0);
	[SerializeField]private bool shiftTextOptions = true;

	//[SerializeField]private int choiceID;
	public List<ChoiceOption> createdOptions = new List<ChoiceOption> (3);

	[HideInInspector][SerializeField]private CanvasGroup cvg;

	public void ShowOptions(bool active = true){
		cvg.alpha = active ? 1f : 0f;
		cvg.interactable = active;
		cvg.blocksRaycasts = active;
	}

	public void Insp_Enabled(){
		if (cvg == null || optionPrefab == null) {
			optionPrefab = Resources.Load ("ResponseButton") as GameObject;		
			cvg = GetComponent<CanvasGroup> ();	
		}

		ShowOptions (true);
	}

	public void Insp_Disabled(){
		ShowOptions (false);
	}

	public void Insp_CreateVisibleOption(int optionIndex, out ChoiceOption option){
		option = new ChoiceOption ();
		Vector3 optionPos = nextOptionPos;
		option.CreateOption (optionPrefab, transform, optionPos);
		option.InitButton (this, optionIndex);
		createdOptions [optionIndex] = option;

		nextOptionPos += optionOffset;
	}

	public void Insp_RemoveOption(int optionIndex){
		Vector3 shiftBackOffset = shiftTextOptions ? optionOffset : Vector3.zero;
		for (int i = optionIndex + 1; i < createdOptions.Count; i++) {
			createdOptions [i].ShiftBack (shiftBackOffset);
		}
		if (shiftTextOptions) {
			nextOptionPos -= optionOffset;
		}

		createdOptions [optionIndex].Remove ();
		createdOptions.RemoveAt (optionIndex);
	}
}