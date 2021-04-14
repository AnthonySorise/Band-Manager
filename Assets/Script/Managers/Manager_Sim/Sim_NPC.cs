using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim_NPC : MonoBehaviour
{
    private List<NPC> _npcs;
    private List<NPC> _npcGraveyard; //Eternal rest grant unto them, O Lord, and let perpetual light shine upon them. May their souls and the souls of all the faithful departed, through the mercy of God, rest in peace. Amen.


    // Start is called before the first frame update
    void Start()
    {
        _npcs = new List<NPC>();
        _npcGraveyard = new List<NPC>();

        BandManager player = new BandManager(NPCGender.Male, 35);
        Debug.Log("Player Created");
        Debug.Log(player.ID + " " + player.Gender.ToString() + " " + player.FirstName + " " + player.LastName + " " + player.BirthDay.ToString() + " ");
    }

    public void StoreNPC(NPC npc)
    {
        _npcs.Insert(0, npc);
    }
    public void KillNPC(NPC npc)
    {
        _npcs.Remove(npc);
        _npcGraveyard.Insert(0, npc);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
