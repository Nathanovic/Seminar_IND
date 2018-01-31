using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

//for buttons that can be clicked on the screen, apart from optionButtons
public class InteractionButton : MonoBehaviour {

	[SerializeField]private string eventName;
	[SerializeField]private string eventParam;

	[HideInInspector]public bool addedOnClickEvent;

	public void Insp_Init(){
		#if UNITY_EDITOR
		if(!addedOnClickEvent){
			Button myButton = GetComponent<Button> ();
			addedOnClickEvent = true;
			UnityAction taskOnClick = System.Delegate.CreateDelegate(typeof(UnityAction), this, "TaskOnClick") as UnityAction;
			UnityEventTools.AddPersistentListener(myButton.onClick, taskOnClick);
		}
		#endif
	}

	void TaskOnClick(){
		if (eventName == "Activate Dialog") {
			Button myButton = GetComponent<Button> ();
			Image myImg = GetComponent<Image> ();
			myButton.enabled = false;
			myImg.enabled = false;
		}
		EventManager.TriggerEvent (eventName, eventParam);
	}
}
