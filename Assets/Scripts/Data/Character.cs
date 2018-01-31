using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterX", menuName = "DataObject/Character", order = 1)]
public class Character : ScriptableObject {

	public int myID = 0;
	public string myName;
	public Sprite mySprite;

	public void Activate(Image image){
		image.enabled = true;
		image.sprite = mySprite;
	}
}
