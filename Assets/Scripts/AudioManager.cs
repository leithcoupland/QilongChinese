using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public enum AudioChannel {Master, Sound, Music};

	public float masterVolumePercent { get; private set; }
	public float musicVolumePercent { get; private set; }
	public float soundVolumePercent { get; private set; }

	AudioSource soundSource;
	AudioSource[] musicSources;
	int activeMusicSourceIndex;

	public static AudioManager instance;

	public AudioClip[] hitSounds; 

	void Awake(){

		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);

			musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++) {
				GameObject newMusicSource = new GameObject ("Music Source " + (i + 1));
				musicSources [i] = newMusicSource.AddComponent<AudioSource> ();
				newMusicSource.transform.parent = transform;
			}

			GameObject newSoundSource = new GameObject ("Sound Source");
			soundSource = newSoundSource.AddComponent<AudioSource> ();
			newSoundSource.transform.parent = transform;

			masterVolumePercent = PlayerPrefs.GetFloat ("Master Volume", 1);
			soundVolumePercent = PlayerPrefs.GetFloat ("Sound Volume", 1);
			musicVolumePercent = PlayerPrefs.GetFloat ("Music Volume", 1);
		}
	}

	public void SetVolume (float volumePercent, AudioChannel channel){

		switch (channel){
		case AudioChannel.Master:
			masterVolumePercent = volumePercent;
			break;
		case AudioChannel.Sound:
			soundVolumePercent = volumePercent;
			break;
		case AudioChannel.Music:
			musicVolumePercent = volumePercent;
			break;
		}

		musicSources [0].volume = musicVolumePercent * masterVolumePercent;
		musicSources [1].volume = musicVolumePercent * masterVolumePercent;

		PlayerPrefs.SetFloat ("Master Volume", masterVolumePercent);
		PlayerPrefs.SetFloat ("Sound Volume", soundVolumePercent);
		PlayerPrefs.SetFloat ("Music Volume", musicVolumePercent);
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1){
		musicSources [activeMusicSourceIndex].loop = false;

		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources [activeMusicSourceIndex].clip = clip;
		musicSources [activeMusicSourceIndex].Play ();
		musicSources [activeMusicSourceIndex].loop = true;

		StartCoroutine (FadeInMusic (fadeDuration));
	}

	public void PlaySound(AudioClip clip){
		soundSource.PlayOneShot (clip, soundVolumePercent * masterVolumePercent);
	}

	public void PlayHitSound(){
		soundSource.PlayOneShot (hitSounds[Random.Range(0, hitSounds.Length)], soundVolumePercent * masterVolumePercent * .7f);
	}

	IEnumerator FadeInMusic(float duration){
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime / duration;
			musicSources [activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, percent);
			musicSources [1-activeMusicSourceIndex].volume = Mathf.Lerp (musicVolumePercent * masterVolumePercent, 0, percent);
			yield return null;
		}
	}

}
