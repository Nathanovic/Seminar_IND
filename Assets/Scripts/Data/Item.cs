using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "DataObject/Item", order = 1)]
public class Item : ScriptableObject {
	public Sprite itemImage;
	public bool canDeplete;
	public float depletionTime = 20f;
}