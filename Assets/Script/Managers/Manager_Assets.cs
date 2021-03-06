﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public enum Asset_png
{  
    None = 0,
    //curosr images
        //should be 32X32, horizontally centered and vertically pushed all the way to the top
    Cursor_Default,
    Cursor_Hover,
    //popup images
    Popup_Vinyl,
    icon_daybox_null,
    icon_daybox_gig,
    icon_daybox_media,
    icon_daybox_produce,
    icon_daybox_scout,
    icon_daybox_special,
    icon_daybox_travel
}

public enum Asset_wav
{
    None = 0,
    //ui Audio
    TimeToggleClick,
    Click_01,
    Click_02,
    Click_03,
    Click_04,
    event_generic
}

public class Manager_Assets : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }


    private Dictionary<Asset_png, Texture2D> _pngTextures;
    private Dictionary<Asset_wav, AudioClip> _wavAudio;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Assets initializing...");

        string resourcesArtFolder = "Art";
        string[] resourcesArtSubFolders = new string[] { "UI" };
        string resourcesAudioFolder = "Audio";
        string[] resourcesAudioSubFolders = new string[] { "UI" };

        string artPath = Application.dataPath + "/Art";
        string audioPath = Application.dataPath + "/Audio";

        var artPathSubFolders = Directory.GetDirectories(artPath);
        var audioPathSubFolders = Directory.GetDirectories(audioPath);

        _pngTextures = new Dictionary<Asset_png, Texture2D>();
        foreach(Asset_png png in Enum.GetValues(typeof(Asset_png)))
        {
            if(png == Asset_png.None)
            {
                continue;
            }

            Texture2D texture = null;
            byte[] fileData = null;
            string fileName = png.ToString().ToLower();
            string filePath = null;
            foreach (var path in artPathSubFolders)
            {
                //Asset Folder User has access to
                filePath = path + "/" + fileName + ".png";
                if (File.Exists(filePath))
                {
                    fileData = File.ReadAllBytes(filePath);
                    texture = new Texture2D(2, 2);
                    texture.LoadImage(fileData);
                }
            }
            if(!texture)
            {
                //Unity Resource Folder User does NOT have access to
                for (var i = 0; i < artPathSubFolders.Length - 1; i++)
                {
                    filePath = resourcesArtFolder + "/" + resourcesArtSubFolders[i] + "/" + fileName;
                    if(File.Exists(Application.dataPath + "/Resources/" + filePath + ".png"))
                    {
                        texture = Resources.Load<Texture2D>(filePath);
                    }
                }
            }
            if (texture)
            {
                //Modify texture import settings based on texture type
                //Alternative -> https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPreprocessTexture.html
                if (png.ToString().Contains("Cursor_"))
                {
                    filePath = AssetDatabase.GetAssetPath(texture);
                    TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(filePath);
                    importer.textureType = TextureImporterType.Cursor;
                }
            }
            else if (!texture)
            {
                Debug.Log("Error - Missing Asset: " + fileName);
                State = ManagerState.Error;
                return;
            }
            else if (_pngTextures.ContainsKey(png))
            {
                Debug.Log("Error - Duplicate Asset Name: " + fileName);
                State = ManagerState.Error;
                return;
            }
            _pngTextures.Add(png, texture);
        }

        _wavAudio = new Dictionary<Asset_wav, AudioClip>();
        foreach (Asset_wav wav in Enum.GetValues(typeof(Asset_wav)))
        {
            if (wav == Asset_wav.None)
            {
                continue;
            }

            AudioClip audioClip = null;
            string fileName = wav.ToString().ToLower();
            string filePath;
            foreach (var path in audioPathSubFolders)
            {
                filePath = path + "/" + fileName + ".wav";
                if (File.Exists(filePath))
                {
                    //WWW is obsloete - doesn't work on Linux
                    //Moving audio assets to Resources folder for now
                    //TO DO -> https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequestMultimedia.GetAudioClip.html
                    //Asset Folder User has access to
                    //audioClip = new WWW(filePath).GetAudioClip(false, true, AudioType.WAV);
                }
            }
            if (!audioClip)
            {
                //Unity Resource Folder User does NOT have access to
                for(var i = 0; i < resourcesAudioSubFolders.Length; i++)
                {
                    filePath = resourcesAudioFolder + "/" + resourcesAudioSubFolders[i] + "/" + fileName;
                    if (File.Exists(Application.dataPath + "/Resources/" + filePath + ".wav"))
                    {
                        audioClip = Resources.Load<AudioClip>(filePath);
                    }
                    if (!audioClip)
                    {
                        Debug.Log("Error - Missing Asset: " + fileName);
                        State = ManagerState.Error;
                        return;
                    }
                }
            }
            if (_wavAudio.ContainsKey(wav))
            {
                Debug.Log("Error - Duplicate Asset Name: " + fileName);
                State = ManagerState.Error;
                return;
            }
            _wavAudio.Add(wav, audioClip);
        }


        State = ManagerState.Started;
        Debug.Log("Manager_Assets started");
    }
	
    public Texture2D GetTexture(Asset_png png)
    {
        if (_pngTextures.ContainsKey(png))
            { return _pngTextures[png]; }
        else
            { return null; }
    }
    public Sprite GetSprite(Asset_png png, float pixelPerUnit = 1f)
    {
        Texture2D texture = GetTexture(png);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 1);
        return newSprite;
    }

    public AudioClip GetAudio(Asset_wav wav)
    {
        if (_wavAudio.ContainsKey(wav))
            { return _wavAudio[wav]; }
        else
            { return null; }
    }
}
