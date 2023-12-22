using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudySpellCast : MonoBehaviour, ISpellCastable
{
    public SpellRecord studiedSpell;
    public int count = 2;
    
    public void SpellCast()
    {
        var player = FindObjectOfType<FieldPlayer>();

        for (int i = 0; i < count; i++)
            player.status.deck.Add(studiedSpell);
        
        player.status.StopStudying();
    }
}
