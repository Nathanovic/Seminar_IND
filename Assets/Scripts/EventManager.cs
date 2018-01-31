using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//event listener for the movement and the addItem events
public class EventManager : MonoBehaviour {

	public delegate void ParamEvent(string eventParam);
	public Dictionary<string, ParamEvent> eventDictionary;

	private static EventManager eventManager;
	public static EventManager instance{
		get{
			if (!eventManager) {
				eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;
				if (!eventManager) {
					Debug.LogError ("There needs to be one active ButtonEventManager script in the scene");
				}
				else {
					eventManager.Init ();
				}
			}

			return eventManager;
		}
	}

	void Init(){
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, ParamEvent> ();
		}
	}

	public static void StartListening(string eventName, ParamEvent listener){
		ParamEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent += listener;
		}
		else {
			thisEvent = listener;
			instance.eventDictionary.Add (eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, ParamEvent listener){
		ParamEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent -= listener;
			if (thisEvent == null) {
				instance.eventDictionary.Remove (eventName);
			}
		}
	}

	public static void TriggerEvent(string eventName, string eventParam){
		ParamEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.Invoke (eventParam);
		}
		else {
			Debug.LogWarning ("event dictionary does not contain: " + eventName);
		}
	}
}