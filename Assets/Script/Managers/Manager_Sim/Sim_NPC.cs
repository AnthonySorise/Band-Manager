using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim_NPC : MonoBehaviour
{
    private TimeSpan _npcBedtime;
    private TimeSpan _npcWakeupTime;

    private int _playerCharacterID;
    private Dictionary<int, NPC> _npcs;
    private Dictionary<int, NPC> _npcGraveyard; //Eternal rest grant unto them, O Lord, and let perpetual light shine upon them. May their souls and the souls of all the faithful departed, through the mercy of God, rest in peace. Amen.


    // Start is called before the first frame update
    void Start()
    {
        _npcBedtime = new TimeSpan(2, 0, 0);
        _npcWakeupTime = new TimeSpan(8, 0, 0);

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
        return _npcs[npcID];
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

    public bool IsSleepTime(DateTime dateTime)
    {
        if (_npcWakeupTime < _npcBedtime)//bed before midnight
        {
            return dateTime.TimeOfDay >= _npcBedtime || dateTime.TimeOfDay < _npcWakeupTime;
        }
        else
        {
            return dateTime.TimeOfDay >= _npcBedtime && dateTime.TimeOfDay < _npcWakeupTime;
        }
    }
    public TimeSpan TimeSinceBedtime(DateTime dateTime)
    {
        if (IsSleepTime(dateTime))
        {
            if (_npcWakeupTime < _npcBedtime && dateTime.TimeOfDay < _npcWakeupTime)//bedtime before midnight and it's after midnight
            {
                return dateTime.TimeOfDay + (TimeSpan.FromDays(1) - dateTime.TimeOfDay);
            }
            else
            {
                return dateTime.TimeOfDay - _npcBedtime;
            }
        }
        else
        {
            return TimeSpan.Zero;
        }
    }
    public DateTime AvoidSleepTime(DateTime startDT, TimeSpan timeSpan)
    {
        DateTime DT = startDT;

        bool startsDuringSleep = Managers.Sim.NPC.IsSleepTime(startDT);
        bool endsDuringSleep = Managers.Sim.NPC.IsSleepTime(startDT + timeSpan);
        bool coversWholeSleepPeriod = !startsDuringSleep && !endsDuringSleep && timeSpan > (_npcBedtime - startDT.TimeOfDay);

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
            DT = DT - timeSpan + (_npcBedtime - startDT.TimeOfDay);
        }

        return DT;
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }
}
