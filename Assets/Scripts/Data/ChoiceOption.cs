using UnityEngine;
using UnityEngine.UI;

//editor-versie van OptionButton
[System.Serializable]
public class ChoiceOption{
	[SerializeField]private GameObject optionObject;
	[SerializeField]private RectTransform rectTransform;
	[SerializeField]private Text optionTextComp;
	[SerializeField]private OptionButton buttonScript;

	[SerializeField]private int optionID{
		get{ 
			return buttonScript.optionID;
		} 
		set{ 
			buttonScript.optionID = value; 
		}
	}
	public string optionLetter;
	public bool removeOption;

	public string buttonText;
	public bool longerDialog;
	public string dialogText;

	public ResponseAction responseAction;

	public void CreateOption(GameObject buttonPrefab, Transform buttonParent, Vector3 buttonRectPos){
		optionObject = GameObject.Instantiate (buttonPrefab, buttonParent);
		rectTransform = optionObject.GetComponent<RectTransform> ();
		optionTextComp = optionObject.GetComponentInChildren<Text> ();
		buttonScript = optionObject.GetComponent<OptionButton> ();

		rectTransform.localPosition = buttonRectPos;
	}

	public void InitButton(ChoiceHolder choiceScript, int letterIndex){
		buttonScript.Init (choiceScript);

		optionID = letterIndex;
		buttonText = "Default Option";

		UpdateText ();
		UpdateName ();
	}

	public void UpdateText(){
		string newDialogText = longerDialog ? dialogText : buttonText;
		buttonScript.dialogText = newDialogText;
		optionTextComp.text = buttonText;
	}

	public void UpdateResponseAction(){
		buttonScript.responseAction = responseAction;
	}

	public void Remove(){
		GameObject.DestroyImmediate (optionObject);
	}

	public void ShiftBack(Vector3 offsetPos){
		optionID--;
		UpdateName ();

		Vector3 myPos = rectTransform.localPosition;
		myPos -= offsetPos;
		rectTransform.localPosition = myPos;
	}

	void UpdateName(){//called after the letter index has changed
		optionLetter = ((OptionLetter)optionID).ToString();
		optionObject.name = "Option " + optionLetter;
	}
}

public enum OptionLetter{
	A,
	B,
	C,
	D,
	E
}