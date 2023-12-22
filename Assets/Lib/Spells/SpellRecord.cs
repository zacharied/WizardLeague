using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellRecord : ScriptableObject
{
     public GameObject spellPrefab = null!;
     public Sprite sprite = null!;
     public string spellName = null!;
     public int spellCost = 0;
}