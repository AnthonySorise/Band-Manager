using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Assets_png
{   
    //curosr images
        //should be 32X32, horizontally centered and vertically pushed all the way to the top
    Cursor_Default,
    Cursor_Hover
}

public class Manager_Assets : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }


    private Dictionary<Assets_png, Texture2D> _pngTextures;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Assets initializing...");


        string artPath = Application.dataPath + "/Art/";
        string audioPath = Application.dataPath + "/Audio/";


        _pngTextures = new Dictionary<Assets_png, Texture2D>();
        foreach(Assets_png png in Enum.GetValues(typeof(Assets_png)))
        {
            Texture2D texture = null;
            byte[] fileData;
            string fileName = png.ToString().ToLower();
            string filePath = artPath + "UI/" + fileName + ".png";
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                _pngTextures.Add(png, texture);
            }
            else
            {
                Debug.Log("Error - Missing Asset: " + filePath);
                State = ManagerState.Error;
            }
        }

        if(State != ManagerState.Error)
        {
            State = ManagerState.Started;
            Debug.Log("Manager_Assets started");
        }
    }
	
    public Texture2D GetPNGTexture(Assets_png png)
    {
        if (_pngTextures.ContainsKey(png))
            { return _pngTextures[png]; }
        else
            { return null; }
    }
}
