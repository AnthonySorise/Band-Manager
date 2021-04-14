using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCTraitMinorID
{

}

public class NPCTraitMinor {
    public NPCTraitMinorID ID;
    private Action<NPC> _apply;
    private Action<NPC> _remove;
    public DateTime ExpirationDate;

    public NPCTraitMinor (NPCTraitMinorID id, Action<NPC> apply, DateTime expirationDate) {
        ID = id;
        _apply = apply;
        ExpirationDate = expirationDate;
    }

    public void Apply (NPC npc)
    {
        _apply(npc); 
    }
    public void Remove (NPC npc)
    {
        _remove(npc);
    }

    public void CheckExpiration (NPC npc) {
        if (Managers.Time.CurrentDT > ExpirationDate) {
            Remove(npc);
        }
    }
}

