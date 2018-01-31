using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

[System.Serializable]
public class OptionButton : MonoBehaviour {

	[SerializeField]private ChoiceHolder choiceScript;

	//[SerializeField]private RectTransform rectTransform;
	//[SerializeField]private Text optionTextComp;

	public int optionID;
	public string dialogText;
	public ResponseAction responseAction;

	public void Init(ChoiceHolder choiceScript){//called when in editor mode
		#if UNITY_EDITOR
		this.choiceScript = choiceScript;

		Button myButton = GetComponent<Button> ();

		UnityAction taskOnClick = System.Delegate.CreateDelegate (typeof(UnityAction), this, "TaskOnClick") as UnityAction;
		UnityEventTools.AddPersistentListener (myButton.onClick, taskOnClick);
		#endif
	}

	void TaskOnClick(){
		choiceScript.ShowOptions (false);
		ResponseManager.instance.ShowDialogTextPlayer (responseAction, dialogText);
	}
}
