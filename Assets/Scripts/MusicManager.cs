using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip gameTheme;
	public AudioClip menuTheme;

	public static MusicManager instance;

	void Awake(){

		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void Start(){
		string sceneName = SceneManager.GetActiveScene ().name;

		if (sceneName == "Menu") {
			PlayMenuMusic ();
		}
		else if (sceneName == "Game"){
			PlayGameMusic ();
		}
	}

	public void PlayMenuMusic(){
		AudioManager.instance.PlayMusic (menuTheme, 1);
	}

	public void PlayGameMusic(){
		AudioManager.instance.PlayMusic (gameTheme, 1);
	}

}
