using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour {

	[Header("Test event by pressing space")]
	public string eventName;
	public string eventParam;

	void Update(){
		if (Input.GetKeyUp (KeyCode.Space)) {
			EventManager.TriggerEvent (eventName, eventParam);
		}
	}
}
