using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;


public enum AudioChannel{
    SFX,
	DX,
    UI,
	Music,
	Ambience
}

public class Manager_Audio : MonoBehaviour, IManager {
    public ManagerState State {get; private set;}

    public AudioMixer mixer;
    //public AudioMixerGroup master;

	private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private GameObject[] _go_Channels = new GameObject[AudioChannel.GetNames(typeof(AudioChannel)).Length];
    private AudioChannel[] _channelsThatLoop = new AudioChannel[]{
        AudioChannel.Music,
        AudioChannel.Ambience
    };

	public void Startup(){
        State = ManagerState.Initializing;
		Debug.Log("Manager_Audio initializing...");
        
        //MIXER
        if(mixer == null){
            mixer = Resources.Load("Mixer") as AudioMixer;
            if (!mixer)
            {
                State = ManagerState.Error;
                Debug.Log("Manager_Audio doesn't have a mixer attached to its mixer property");
                return;
            }
        }

        //CHANNEL GAMEOBJECTS
        foreach (AudioChannel channel in AudioChannel.GetValues(typeof(AudioChannel)))
        {
            //create channel go
            GameObject GO_Channel = new GameObject();
            GO_Channel.transform.parent = GameObject.Find("Managers").transform;
            GO_Channel.name = "Audio_" + channel.ToString();
            //store go in private array
            _go_Channels[(int)channel] = GO_Channel;
        }

        
        State = ManagerState.Started;
        Debug.Log("Manager_Audio started...");
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

    //AUDIO
    private AudioSource GetAudioSource(GameObject gameObject)
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        return gameObject.GetComponent<AudioSource>();
    }

    private void PlayAudioSource(AudioSource audioSource, AudioClip clip, AudioChannel group, float spatialBlend, float volume, bool loop)
    {
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(group.ToString())[0];
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    public void PlayAudio(Asset_wav wav, AudioChannel audioChannel, GameObject go = null){
        AudioClip audioClip = Managers.Assets.GetAudio(wav);
        float spatialBlend;
        if(go == null){
            go = _go_Channels[(int)audioChannel];
            spatialBlend = 0;
        }
        else{
            spatialBlend = 1;
        }
        if(audioClip == null){
            Debug.LogError(go.name + " failed to play audio: " + wav.ToString());
            return;
        }
        AudioSource audioSource = GetAudioSource(go);
        bool doesLoop = _channelsThatLoop.Contains(audioChannel);
        PlayAudioSource(audioSource, audioClip, audioChannel, spatialBlend, 1, doesLoop);
    }
}