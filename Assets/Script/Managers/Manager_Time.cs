﻿using System;
using System.Collections;
using UnityEngine;

public class Manager_Time : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    //ticks
    public DateTime CurrentDT { get; private set; }
    public bool IsPaused { get; private set; }
    private bool _IsDelayedWhileTickProcesses;
    private int _simMinutesPerTick;
    private int _simMinutesSinceLastTick;

    //game speed
    public int CurrentSpeedLevel { get; private set; }
    private int _maxSpeedLevel;
    private int _minSpeedLevel;
    private int _baseMSPerSimMinute;
    private double _speedLevelDenominator;
    private float _secondsToWait;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Time initializing...");

        IsPaused = true;
        _IsDelayedWhileTickProcesses = false;
        _simMinutesPerTick = 11;
        _simMinutesSinceLastTick = 0;
        _baseMSPerSimMinute = 250;
        _speedLevelDenominator = 2.5;
        CurrentSpeedLevel = 1;
        _maxSpeedLevel = 5; //min = 0

        //temporary, will need to be initated elsewhere
        CurrentDT = new DateTime(1985, 10, 23, 0, 0, 0);
        //

        State = ManagerState.Started;
        Debug.Log("Manager_Time started");
    }

    public void SetSimDT(int year, int month, int day, int hrs, int mins)
    {
        Pause();
        CurrentDT = new DateTime(year, month, day, hrs, mins, 0);
    }

    public void ToggleTime()
    {
        if (CurrentDT != null)
        {
            if (IsPaused)
            {
                Play();
            }
            else
            {
                Pause();
            }
            Managers.Audio.PlayAudio(Asset_wav.TimeToggleClick, AudioChannel.UI);
        }
        else
        {
            Debug.Log("Error: Cannot ToggleTime() when CurrentDT is null");
        }
    }

    public void Play()
    {
        IsPaused = false;
        IncrementDT();
    }

    public void Pause()
    {
        IsPaused = true;
        StopCoroutine("WaitAndIncrementDT");
    }

    private int RealMSPerSimMinute()
    {
        int ms = _baseMSPerSimMinute;
        for (int i = 0; i < CurrentSpeedLevel; i++)
        {
            ms = Convert.ToInt32(ms / _speedLevelDenominator);
        }
        return ms;
    }
    private int NumMinutesToIncrementDt()
    {
        switch (CurrentSpeedLevel)
        {
            case 3:
                return 2;
            case 4:
                return 2;
            case 5:
                return 7;
            default:
                return 1;
        }
    }
    private void IncrementDT()
    {
        if (!_IsDelayedWhileTickProcesses)
        {
            CurrentDT = CurrentDT.AddMinutes(NumMinutesToIncrementDt());
            _simMinutesSinceLastTick += NumMinutesToIncrementDt();
        }

        StartCoroutine("WaitAndIncrementDT");

        if (_simMinutesSinceLastTick >= _simMinutesPerTick || _IsDelayedWhileTickProcesses)
        {
            if (!Managers.Model.IsProcessingTick)
            {
                _IsDelayedWhileTickProcesses = false;
                _simMinutesSinceLastTick = 0;
                StartCoroutine("PlayTick");
            }
            else
            {
                _IsDelayedWhileTickProcesses = true;
            }
        }
    }
    IEnumerator WaitAndIncrementDT()
    {
        var timeToWait = (RealMSPerSimMinute() / 1000f) * NumMinutesToIncrementDt();
        yield return new WaitForSecondsRealtime(timeToWait);
        if (!IsPaused)
        {
            IncrementDT();
        }
    }
    IEnumerator PlayTick()
    {
        Managers.Model.SimulateTick();
        return null;
    }

    public void IncreaseSpeed()
    {
        if (CurrentSpeedLevel < _maxSpeedLevel)
        {
            CurrentSpeedLevel++;
            Managers.Audio.PlayAudio(Asset_wav.GenericClick, AudioChannel.UI);
        }
    }
    public void DecreaseSpeed()
    {
        if (CurrentSpeedLevel > 0)
        {
            CurrentSpeedLevel--;
            Managers.Audio.PlayAudio(Asset_wav.GenericClick, AudioChannel.UI);
        }
    }
}
