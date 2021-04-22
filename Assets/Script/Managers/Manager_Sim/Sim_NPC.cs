using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim_NPC : MonoBehaviour
{
    private Dictionary<int, NPC> _npcs;
    private Dictionary<int, NPC> _npcGraveyard; //Eternal rest grant unto them, O Lord, and let perpetual light shine upon them. May their souls and the souls of all the faithful departed, through the mercy of God, rest in peace. Amen.


    // Start is called before the first frame update
    void Start()
    {
        _npcs = new Dictionary<int, NPC>();
        _npcGraveyard = new Dictionary<int, NPC>();

        NPC_BandBanager player = new NPC_BandBanager(NPCGender.Male, 35, CityID.Detroit_MI);
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

    public NPC getNPC(int npcID)
    {
        return _npcs[npcID];
    }
    public NPC_BandBanager getPlayerCharacter()
    {
        NPC_BandBanager playerCharacter = _npcs[1] as NPC_BandBanager;
        return playerCharacter;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
