using UnityEngine;

public class ConditionalInvoker : InteractionButton {

	private bool requireSomething = true;
	[SerializeField]private ConditionType conditionType;
	[SerializeField]private string requirementName;

	[SerializeField]private string alternateEvent;
	[SerializeField]private string alternateParam;

	protected override void TaskOnClick (){
		if (requireSomething) {
			if (conditionType == ConditionType.RequireItem) {
				if (Inventory.instance.ItemInInventory (requirementName)) {
					Debug.Log ("item is in inventory: " + requirementName);
					requireSomething = false; 
				} else {
					EventManager.TriggerEvent (alternateEvent, alternateParam);
					return;
				}
			} else {
				if (!GameManager.instance.EventHasPassed (requirementName)) {
					EventManager.TriggerEvent (alternateEvent, alternateParam);
					return;
				}
			}
		}
			
		base.TaskOnClick ();
	}
}

public enum ConditionType{
	RequireItem,
	RequireEvent
}