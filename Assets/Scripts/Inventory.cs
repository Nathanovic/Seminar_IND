using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	public Item[] availableItems = new Item[5];

	public Image[] itemImages;
	public Text[] itemCountTexts;

	public List<Item> allItems = new List<Item> (5);//public QQQ
	public List<int> itemCounts = new List<int> (5);//public QQQ

	private CanvasGroup inventoryContentCVG;
	private GameObject inventoryButtonObject;
	private bool inventoryOpen = true;//is toggled to false in Start()

	void Start(){
		EventManager.StartListening ("Add Item", TryAddItem);
		EventManager.StartListening ("Remove Item", TryRemoveItem);

		inventoryContentCVG = transform.GetChild (0).GetComponent<CanvasGroup> ();
		inventoryButtonObject = transform.GetChild (1).gameObject;

		ResponseManager.instance.onDialogStarted += HideInventory;
		ResponseManager.instance.onDialogEnded += ShowInventory;

		ToggleInventory ();
	}

	void TryAddItem(string itemName){
		Item item = ItemFromName (itemName);
		if (item != null) {
			AddItem (item);
		}
		else {
			Debug.Log ("failed in adding " + itemName);
		}
	}

	void AddItem(Item item){
		int itemIndex = allItems.Count;
		if (allItems.Contains (item)) {
			itemIndex = allItems.IndexOf (item);
			itemCounts [itemIndex]++;
		}
		else {
			allItems.Add (item);
			itemCounts.Add (1);
			itemImages [itemIndex].sprite = item.itemImage;
		}

		itemCountTexts [itemIndex].text = ItemCountText(itemIndex);
	}

	void TryRemoveItem(string itemName){
		Item item = ItemFromName (itemName);
		if (item != null) {
			RemoveItem (item);
		}
		else {
			Debug.Log ("failed in removing " + itemName);
		}
	}

	void RemoveItem(Item item){
		int itemIndex = allItems.IndexOf (item);
		itemCounts [itemIndex] --;
		if (itemCounts [itemIndex] == 0) {
			int lastItemIndex = allItems.Count - 1;
			allItems.RemoveAt (itemIndex);
			itemCounts.RemoveAt (itemIndex);	

			for (int i = 0; i < lastItemIndex; i++) {
				itemImages [i].sprite = allItems [i].itemImage;
				itemCountTexts [i].text = itemCounts [i].ToString();
			}

			itemImages [lastItemIndex].sprite = null;
			itemCountTexts [lastItemIndex].text = "";
		} else {
			itemCountTexts [itemIndex].text = ItemCountText(itemIndex);
		}
	}

	Item ItemFromName(string itemName){
		for (int i = 0; i < availableItems.Length; i++) {
			if (availableItems [i].name == itemName) {
				return availableItems [i];
			}
		}

		return null;
	}

	void ShowInventory(){
		inventoryButtonObject.SetActive (true);
	}

	void HideInventory(){
		inventoryButtonObject.SetActive (false);		
	}

	public void ToggleInventory(){
		inventoryOpen = !inventoryOpen;
		inventoryContentCVG.alpha = inventoryOpen ? 1f : 0f;
		inventoryContentCVG.interactable = inventoryOpen;
		inventoryContentCVG.blocksRaycasts = inventoryOpen;
	}

	string ItemCountText(int itemIndex){
		int count = itemCounts [itemIndex];
		if (count == 1) {
			return "";
		} else {
			return count.ToString ();
		}
	}
}
