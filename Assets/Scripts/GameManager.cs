using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//this script manages the state of different things of the game
//herein are all parameters(except for keys) that determine whether dialog 7 can start, for example
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private List<string> passedEventList;

	[SerializeField]private Image endingImg;
	[SerializeField]private Sprite[] endingSprites;

	public delegate void ManagerDelegate ();
	public event ManagerDelegate onNotAloneEvent;
	public delegate void EndGameEvent(bool goodEnding);
	public event EndGameEvent onGameEnd;

	[SerializeField]private Dialog endingDialog;
	[SerializeField]private ResponseAction[] endingResponses;

	[SerializeField]private GameObject[] interactableButtons;//disable these onGameEnd
	[SerializeField]private GameObject menuButton;

	void Awake(){
		instance = this;
	}

	void Start () {
		passedEventList = new List<string> (5);
		EventManager.StartListening ("Game Event", GameEventCalled);
		EventManager.StartListening ("End Game", ChooseGameEnding);
	}

	void GameEventCalled(string passedEvent){
		if (passedEvent == "Not Alone" && onNotAloneEvent != null) {
			onNotAloneEvent ();
		}
		passedEventList.Add (passedEvent);
		Debug.Log ("Event passed: " + passedEvent);
	}

	void ChooseGameEnding(string gameEnding){
		int gameEndID = int.Parse (gameEnding);
		Debug.Log ("Game end: " + gameEndID);

		endingImg.enabled = true;
		endingImg.sprite = endingSprites [gameEndID];

		endingDialog.npcDialogStarter = endingResponses[gameEndID];
		endingDialog.Activate ();

		ResponseManager.instance.onDialogEnded += GameEnd;

		if (gameEndID == 3)
			onGameEnd (true);
		else
			onGameEnd (false);
	}

	public bool EventHasPassed(string eventName){
		if (passedEventList.Contains (eventName)) {
			return true;
		}

		return false;
	}

	void GameEnd(){
		for (int i = 0; i < interactableButtons.Length; i++) {
			interactableButtons [i].SetActive(false);
		}

		menuButton.SetActive (true);
	}

	public void ReturnToMenu(){
		SceneManager.LoadScene (0);
	}
}
