using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Audio : MonoBehaviour, IManager {
	public ManagerState State {get; private set;}
	private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

	public void Startup(){
		Debug.Log("Manager_Audio Starting...");

		Object[] audioClipArray = Resources.LoadAll("Audio/");
		foreach (Object clip in audioClipArray){
			_audioClips.Add(clip.name, clip as AudioClip);
		}

		State = ManagerState.Started;
	}
}