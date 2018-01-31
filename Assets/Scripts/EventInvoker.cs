using UnityEngine;

//deze kan gebruikt worden om een event aan te roepen! :)
public class EventInvoker {
	public string eventName;
	public string eventParam;

	public void Trigger(){
		EventManager.TriggerEvent (eventName, eventParam);
	}
}