using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;
using System.IO;

public class Menu : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public GameObject questionSelectHolder;

	public Slider[] volumeSliders;
	public Toggle fullscreenToggle;
	public Toggle windowedToggle;

	const string DROPDOWN_PROMPT = "<Select question file>";

	void Start(){
		bool isFullScreen = (PlayerPrefs.GetInt ("Fullscreen") == 1);

		volumeSliders [0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders [1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [2].value = AudioManager.instance.soundVolumePercent;

		fullscreenToggle.isOn = isFullScreen;
		windowedToggle.isOn = !isFullScreen;

		MainMenu ();
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (!mainMenuHolder.activeInHierarchy){
				MainMenu ();
			} else{
				Quit ();
			}

		}
	}

	public void Play(){
		LoadFileOptions ();
		QuestionSelect ();
	}

	public void StartGame(){
		Dropdown dropdown = questionSelectHolder.GetComponentInChildren<Dropdown> ();
		if (dropdown.value != 0) {
			QuestionParser.instance.SetupFlashcards (dropdown.value-1);
			//Debug.Log ("DROPDOWN VAL : " + dropdown.value);
			MusicManager.instance.PlayGameMusic();
			SceneManager.LoadScene ("Game");
		}
	}

	public void Quit(){
		Application.Quit ();
	}

	public void OptionsMenu(){
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
		questionSelectHolder.SetActive (false);
	}

	public void MainMenu(){
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
		questionSelectHolder.SetActive (false);
	}

	public void QuestionSelect(){
		questionSelectHolder.SetActive (true);
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (false);
	}

	public void SetFullscreen(bool isFullScreen){
		if (isFullScreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxResolution.width, maxResolution.height, true);
		} else {
			Screen.SetResolution (1280, 720, false);
		}
		PlayerPrefs.SetInt ("Fullscreen", isFullScreen?1:0);
		PlayerPrefs.Save ();
	}

	public void SetWindowed(bool isWindowed){
		if (isWindowed) {
			SetFullscreen (false);
		} else {
			SetFullscreen (true);
		}
	}

	public void SetMasterVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSoundVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sound);
	}

	void LoadFileOptions(){
		FileInfo[] fileList = QuestionParser.instance.CheckFolder ();
		List<string> dropdownOptions = new List<string> ();
		dropdownOptions.Add (DROPDOWN_PROMPT);
		foreach (FileInfo f in fileList){
			dropdownOptions.Add (f.Name);
		}
		Dropdown dropdown = questionSelectHolder.GetComponentInChildren<Dropdown> ();
		dropdown.ClearOptions ();
		dropdown.AddOptions (dropdownOptions);
	}

}
