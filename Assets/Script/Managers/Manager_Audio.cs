using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Manager_Audio : MonoBehaviour, IManager {
    public ManagerState State {get; private set;}

    public AudioMixer mixer;
    //public AudioMixerGroup master;
	private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    
	public void Startup(){
        State = ManagerState.Initializing;
		Debug.Log("Manager_Audio Initializing...");

        if(mixer == null){
            State = ManagerState.Error;
            Debug.Log("Manager_Audio doesn't have a mixer attached to its mixer property");
            return;
        }

		Object[] audioClipArray = Resources.LoadAll("Audio/");
		foreach (Object clip in audioClipArray){
			_audioClips.Add(clip.name, clip as AudioClip);
		}

		State = ManagerState.Started;
	}

    //MIXER

    public void ChangeMixerVolume(string group, float vol){
        if(mixer.FindMatchingGroups(group).Length == 0){
            Debug.Log("Mixer Group " + group + " doesn't exist");
            return;
        }

        float currentVol;
        mixer.GetFloat(group, out currentVol);
        float newVol = currentVol + (Mathf.Log10(vol) * 20f);
        if(newVol < -80f){
            newVol = -80f;
        }
        else if(newVol > 20f){
            newVol = 20f;
        }
        mixer.SetFloat(group, newVol);
    }

    public void SetMixerVolume(string group, float vol){
        if(mixer.FindMatchingGroups(group).Length == 0){
            Debug.Log("Mixer Group " + group + " doesn't exist");
            return;
        }

        mixer.SetFloat(group, Mathf.Log10(vol) * 20f);
    }

    //TRIGGER

	//Get Audio Source Component from Game Object
    private AudioSource GetAudioSource(GameObject gameObject)
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        return gameObject.GetComponent<AudioSource>();
    }

    //Play AudioSource
    private void PlayAudioSource(AudioSource audioSource, AudioClip clip, string group, int volume, int spatialBlend, bool loop)
    {
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(group)[0];
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }







}