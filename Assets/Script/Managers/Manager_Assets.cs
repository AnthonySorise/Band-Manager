using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Manager_Assets : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    private enum UIImage
    {
        Cursor_Default,
        Cursor_Hover
    }
    private Dictionary<UIImage, Texture2D> _uiImages;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Assets initializing...");



        string artPath = Application.dataPath + "/Art/";
        string audioPath = Application.dataPath + "/Audio/";



        _uiImages = new Dictionary<UIImage, Texture2D>();
        foreach(UIImage image in Enum.GetValues(typeof(UIImage)))
        {
            Texture2D texture = null;
            byte[] fileData;
            string fileName = image.ToString().ToLower();
            string filePath = artPath + "UI/" + fileName + ".png";
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                _uiImages.Add(image, texture);
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
	
	void Update () {
		
	}
}
