using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Asset_png
{   
    //curosr images
        //should be 32X32, horizontally centered and vertically pushed all the way to the top
    Cursor_Default,
    Cursor_Hover
}

public enum Asset_wav
{
    //ui Audio
    MenuOpen,
    GenericClick,
    TimeToggleClick
}

public class Manager_Assets : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }


    private Dictionary<Asset_png, Texture2D> _pngTextures;
    private Dictionary<Asset_wav, AudioClip> _wavAudio;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Assets initializing...");


        string artPath = Application.dataPath + "/Art/";
        string audioPath = Application.dataPath + "/Audio/";

        var artPathSubFolders = Directory.GetDirectories(artPath);
        var audioPathSubFolders = Directory.GetDirectories(audioPath);

        _pngTextures = new Dictionary<Asset_png, Texture2D>();
        foreach(Asset_png png in Enum.GetValues(typeof(Asset_png)))
        {
            Texture2D texture = null;
            byte[] fileData;
            string fileName = png.ToString().ToLower();

            foreach (var path in artPathSubFolders)
            {
                string filePath = path + "/" + fileName + ".png";
                if (File.Exists(filePath))
                {
                    fileData = File.ReadAllBytes(filePath);
                    texture = new Texture2D(2, 2);
                    texture.LoadImage(fileData);
                    if (!_pngTextures.ContainsKey(png))
                    {
                        _pngTextures.Add(png, texture);
                    }
                    else
                    {
                        Debug.Log("Error - Duplicate Asset Name: " + fileName);
                        State = ManagerState.Error;
                        return;
                    }
                }
            }
            if(!texture)
            {
                Debug.Log("Error - Missing Asset: " + fileName);
                State = ManagerState.Error;
                return;
            }
        }

        _wavAudio = new Dictionary<Asset_wav, AudioClip>();
        foreach (Asset_wav wav in Enum.GetValues(typeof(Asset_wav)))
        {
            AudioClip audioClip = null;
            string fileName = wav.ToString().ToLower();

            foreach (var path in audioPathSubFolders)
            {
                string filePath = path + "/" + fileName + ".wav";
                if (File.Exists(filePath))
                {
                    audioClip = new WWW(filePath).GetAudioClip(false, true, AudioType.WAV);
                    if (!_wavAudio.ContainsKey(wav))
                    {
                        _wavAudio.Add(wav, audioClip);
                    }
                    else
                    {
                        Debug.Log("Error - Duplicate Asset Name: " + fileName);
                        State = ManagerState.Error;
                        return;
                    }
                }
            }
            if (!audioClip)
            {
                Debug.Log("Error - Missing Asset: " + fileName);
                State = ManagerState.Error;
                return;
            }
        }


        State = ManagerState.Started;
        Debug.Log("Manager_Assets started");
    }
	
    public Texture2D GetPNGTexture(Asset_png png)
    {
        if (_pngTextures.ContainsKey(png))
            { return _pngTextures[png]; }
        else
            { return null; }
    }
    public AudioClip GetAudio(Asset_wav wav)
    {
        if (_wavAudio.ContainsKey(wav))
        { return _wavAudio[wav]; }
        else
        { return null; }
    }
}
