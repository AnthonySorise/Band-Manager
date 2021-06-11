using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim_NPC : MonoBehaviour
{
    private TimeSpan _npcBedtime;
    private TimeSpan _npcSleepDuration;

    private int _playerCharacterID;
    private Dictionary<int, NPC> _npcs;
    private Dictionary<int, NPC> _npcGraveyard; //Eternal rest grant unto them, O Lord, and let perpetual light shine upon them. May their souls and the souls of all the faithful departed, through the mercy of God, rest in peace. Amen.


    // Start is called before the first frame update
    void Start()
    {
        _npcBedtime = new TimeSpan(2, 0, 0);
        _npcSleepDuration = TimeSpan.FromHours(6);

        _playerCharacterID = 1;
        _npcs = new Dictionary<int, NPC>();
        _npcGraveyard = new Dictionary<int, NPC>();

        NPC_BandManager player = new NPC_BandManager(NPCGender.Male, 35, CityID.Detroit_MI);
        Debug.Log("Player Created");
        Debug.Log(player.ID + " " + player.Gender.ToString() + " " + player.FirstName + " " + player.LastName + " " + player.BirthDay.ToString() + " ");
    }

    public void StoreNPC(NPC npc)
    {
        _npcs.Add(npc.ID, npc);
    }
    public void KillNPC(NPC npc)
    {
        _npcs.Remove(npc.ID);
        _npcGraveyard.Add(npc.ID, npc);
    }

    public NPC GetNPC(int npcID)
    {
        return _npcs!=null ? _npcs.ContainsKey(npcID) ? _npcs[npcID] : null : null;
    }
    public NPC_BandManager GetPlayerCharacter()
    {
        NPC_BandManager playerCharacter = _npcs[_playerCharacterID] as NPC_BandManager;
        return playerCharacter;
    }
    public int PlayerCharacterID()
    {
        return _playerCharacterID;
    }
    public bool IsPlayerCharacter(int npcID)
    {
        return (npcID == _playerCharacterID);
    }

    public DateTime NextBedtime()
    {
        if(Managers.Time.CurrentDT.TimeOfDay < (_npcBedtime + _npcSleepDuration))
        {
            return Managers.Time.CurrentDT.Date + _npcBedtime;
        }
        else
        {
            return Managers.Time.CurrentDT.Date + _npcBedtime + TimeSpan.FromDays(1);
        }
    }
    public DateTime NextWaketime()
    {
        return NextBedtime() + _npcSleepDuration;
    }

    public bool IsOverlappedWithSleepTime(DateTime dateTime, TimeSpan duration)
    {
        DateTime sleepStart = dateTime.Date + _npcBedtime;
        DateTime sleepEnd = dateTime.Date + _npcBedtime + _npcSleepDuration;
        DateTime eventStart = dateTime;
        DateTime eventEnd = dateTime + duration;

        DateTime wakeupTime = dateTime.Date + _npcBedtime + _npcSleepDuration;
        if (dateTime > wakeupTime)
        {
            sleepStart = sleepStart.AddDays(1);
            sleepEnd = sleepEnd.AddDays(1);
        }
        return sleepStart < eventEnd && eventStart < sleepEnd;
    }
    public TimeSpan TimeSinceBedtime(DateTime dateTime)
    {
        if (IsOverlappedWithSleepTime(dateTime, TimeSpan.Zero))
        {
            DateTime bedTime = dateTime.Date + _npcBedtime;
            DateTime wakeupTime = bedTime + _npcSleepDuration;
            if (dateTime > wakeupTime)
            {
                bedTime.AddDays(1);
            }
            return dateTime - bedTime;
        }
        else
        {
            return TimeSpan.Zero;
        }
    }
    public DateTime AvoidSleepTime(DateTime startDT, TimeSpan timeSpan)
    {
        DateTime DT = startDT;

        bool startsDuringSleep = IsOverlappedWithSleepTime(startDT, TimeSpan.Zero);
        bool endsDuringSleep = IsOverlappedWithSleepTime(startDT + timeSpan, TimeSpan.Zero);
        bool coversWholeSleepPeriod = !startsDuringSleep && !endsDuringSleep && IsOverlappedWithSleepTime(startDT, TimeSpan.Zero);

        if (startsDuringSleep)
        {
            DT = DT - TimeSinceBedtime(startDT) - timeSpan;
        }
        else if (endsDuringSleep)
        {
            DT = DT - TimeSinceBedtime(DT);
        }
        else if (coversWholeSleepPeriod)
        {
            DT = (startDT.Date + _npcBedtime) - timeSpan;
        }

        return DT;
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }
}
