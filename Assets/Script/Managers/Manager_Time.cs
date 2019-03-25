using System;
using System.Collections;
using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class Manager_Time : MonoBehaviour, IManager {
    public ManagerState State { get; private set; }

    //ticks
    public DateTime CurrentDT { get; private set; }
    public bool IsPaused { get; private set; }
    private int _simMinutesPerTick;

    //game speed
    public int CurrentSpeedLevel { get; private set; }
    public int MaxSpeedLevel { get; private set; }
    public int MinSpeedLevel { get; private set; }
    private int _baseMSPerTick;
    private double _speedLevelDenominator;
    private float _secondsToWait;


    public void Startup()
    {
        State = ManagerState.Initializing;
        Debug.Log("Manager_Time initializing...");

        IsPaused = true;
        _simMinutesPerTick = 10;
        _baseMSPerTick = 1000;
        _speedLevelDenominator = 2.5;
        CurrentSpeedLevel = 1;
        MaxSpeedLevel = 5;

        //temporary, will need to be initated elsewhere
        CurrentDT = new DateTime(1985, 10, 23, 0, 0, 0);
        //

        State = ManagerState.Started;
        Debug.Log("Manager_Time started");
    }

    public void SetSimDT(int year, int month, int day, int hrs, int mins)
    {
        CurrentDT = new DateTime(year, month, day, hrs, mins, 0);
        IsPaused = true;
    }

    public void ToggleTime()
    {
        if (CurrentDT != null) {
            if (IsPaused == true)
            {
                IsPaused = false;
                PlayTick();
            }
            else
            {
                StopCoroutine("FinishTickAndPlayNewTick");
                IsPaused = true;
            }
        }
        else
        {
            Debug.Log("Error: Cannot ToggleTime() when CurrentDT is null");
        }
    }

    public void PlayTick()
    {
        if (IsPaused == false)
        {
            DateTime startTime = DateTime.Now;

            CurrentDT = CurrentDT.AddMinutes(_simMinutesPerTick);
            Managers.Model.SimulateTick();

            DateTime endTime = DateTime.Now;
            TimeSpan elapsedTime = endTime.Subtract(startTime);

            if (elapsedTime.Milliseconds < MSPerTick())
            {
                _secondsToWait = (float)(MSPerTick() - elapsedTime.Milliseconds) / 1000f;
                StartCoroutine("TrackSecondToWait");
                StartCoroutine("FinishTickAndPlayNewTick");
            }
            else
            {
                PlayTick();
                Debug.Log("Desired speed net met due to overtaxed resources");
            }
        }
    }
    IEnumerator TrackSecondToWait()
    {
        yield return new WaitForSeconds(0.01f);
        _secondsToWait -= 0.01f;
        if (_secondsToWait > 0)
        {
            StartCoroutine("TrackSecondToWait");
            
        }
    }
    IEnumerator FinishTickAndPlayNewTick()
    {
        yield return new WaitForSeconds(_secondsToWait);
        PlayTick();
    }

    private int MSPerTick()
    {
        int ms = _baseMSPerTick;
        for (var i = 0; i < CurrentSpeedLevel; i++)
        {
            ms = Convert.ToInt32(ms / _speedLevelDenominator);
        }
        return ms;
    }

    public void IncreaseSpeed()
    {
        if (CurrentSpeedLevel < MaxSpeedLevel)
        {
            float oldSecondsPerTick = MSPerTick() / 1000f;
            CurrentSpeedLevel++;
            float newSecondsPerTick = MSPerTick() / 1000f;
            if (IsPaused == false)
            {
                ReadjustSecondsToWait(newSecondsPerTick - oldSecondsPerTick);
            }
        }
    }
    public void DecreaseSpeed()
    {
        if (CurrentSpeedLevel > 0)
        {
            float oldSecondsPerTick = MSPerTick() / 1000f;
            CurrentSpeedLevel--;
            float newSecondsPerTick = MSPerTick() / 1000f;
            if (IsPaused == false)
            {
                ReadjustSecondsToWait(newSecondsPerTick - oldSecondsPerTick);
            }
        }
    }
    private void ReadjustSecondsToWait(float adjustment)
    {
        StopCoroutine("FinishTickAndPlayNewTick");

        _secondsToWait += adjustment;
        if (_secondsToWait > 0)
        {
            StartCoroutine("FinishTickAndPlayNewTick");
        }
        else
        {
            StopCoroutine("TrackSecondToWait");
            PlayTick();
        }
    }
}
