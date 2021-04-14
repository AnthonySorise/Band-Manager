using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCTraitMajorID
{

}

public class NPCTraitMajor {
    public NPCTraitMajorID ID;
    private Action<NPC> _apply;
    private Action<NPC> _remove;

    public NPCTraitMajor (NPCTraitMajorID id, Action<NPC> apply)
    {
        ID = id;
        _apply = apply;
    }

    public void Apply (NPC npc)
    {
        _apply(npc);
    }
    public void Remove (NPC npc) {
        _remove(npc);
    }
}
