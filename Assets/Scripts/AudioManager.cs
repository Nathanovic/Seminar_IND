using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private AudioSource mySource;
	private AudioSource musicSource;
	[SerializeField]private AudioClip currentBGClip;
	[SerializeField]private AudioClip creepyClip2;

	[SerializeField]private AudioClip goodEndingClip;
	[SerializeField]private AudioClip badEndingClip;

	void Start () {
		ResponseManager.instance.onDialogStarted += LowerSoundVolume;
		ResponseManager.instance.onDialogEnded += IncreaseSoundVolume;
		GameManager.instance.onNotAloneEvent += IncreaseCreepyness;

		GameManager.instance.onGameEnd += PlayEndGameAudio;

		mySource = GetComponent<AudioSource> ();
		musicSource = transform.GetChild (0).GetComponent<AudioSource> ();

		StartCoroutine (LoopSound());
	}

	void LowerSoundVolume(){//called on DialogStarted
		mySource.volume = 0.5f;
	}

	void IncreaseSoundVolume(){//called on DialogEnded
		mySource.volume = 1f;
	}

	void IncreaseCreepyness(){//called on NotAloneEvent
		currentBGClip = creepyClip2;
		musicSource.volume = .75f;
	}

	IEnumerator LoopSound(){
		while (true) {
			yield return new WaitUntil (() => mySource.isPlaying == false);
			mySource.clip = currentBGClip;
			mySource.Play ();
		}
	}

	void PlayEndGameAudio(bool goodEnd){
		musicSource.volume = 1f;
		if (goodEnd) {
			mySource.volume = 0.15f;
			musicSource.clip = goodEndingClip;
		} else {
			musicSource.clip = badEndingClip;
		}
		musicSource.Play ();
		Debug.Log ("new clip: " + musicSource.clip.name);
	}
}
