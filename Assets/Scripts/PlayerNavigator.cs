using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerNavigator : MonoBehaviour {

	public ScreenInteractor[] allLocations;//public QQQ

	private CanvasGroup currentBackground;

	public delegate void NavigationDelegate ();
	public NavigationDelegate onDestinationReached;
	public NavigationDelegate onNavigateToNewLoc;

	public int startLocation;

	IEnumerator Start () {
		EventManager.StartListening ("Navigate", MoveToLocation);

		yield return null;

		allLocations [startLocation - 1].NavigateToMe ();
		allLocations [startLocation - 1].DestinationReached ();
	}

	void MoveToLocation(string newLoc){
		if(onNavigateToNewLoc != null)
			onNavigateToNewLoc ();//used for smooth transition

		int newLocIndex = int.Parse (newLoc);//letterlijk het getal dat op de map staat
		if (newLocIndex <= allLocations.Length) {
			newLocIndex--;//maak er een echt index getal van
			allLocations [newLocIndex].NavigateToMe ();
		} else {
			Debug.Log ("Location: " + newLocIndex + " doesn't exist...");
		}
	}

	public void DestinationReached(){
		if(onDestinationReached != null)
			onDestinationReached ();
	}
}
