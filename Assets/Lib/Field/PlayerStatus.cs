using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Lib;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Spellcasting))]
public class PlayerStatus : MonoBehaviour
{
    internal const int SpellSlotCount = 3;
    internal const float SpellStockDelay = 0.25f;
    internal const float DeckReloadDelay = 0.75f;
    
    internal const float ManaTickProgressPerSecond = 0.3f;
    internal const int MaxMana = 9;
    internal const int StartingMana = 9;

    internal ulong playerId = 0;
    internal int playerNumber = 1;
    
    public List<SpellRecord> startingDeck = new();
    internal List<SpellRecord> deck = new() { };
    
    internal int CurrentMana;
    internal float NextManaTickProgress;

    internal SpellSlots spellSlots = new(SpellSlotCount);

    internal bool isReloadingDeck { get; private set; }= false;
    internal bool isRestockingSpell { get; private set; } = false;
    internal bool isStudying = false;
    
    void Start()
    {
        deck = new(startingDeck);
        CurrentMana = StartingMana;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMana();
        
        if (!isStudying) {
            // Fill an empty slot
            if (!isRestockingSpell && spellSlots.HasEmptySlot()) {
                Invoke(nameof(RestockSpell), SpellStockDelay);
                isRestockingSpell = true;
            }

            // Reload deck if done
            if (!isReloadingDeck && !spellSlots.HasAny() && !deck.Any()) {
                Invoke(nameof(ReloadDeck), DeckReloadDelay);
                isReloadingDeck = true;
            }
        }
        
    }
    void UpdateMana()
    {
        if (NextManaTickProgress >= 1f) {
            if (IncrementMana()) {
                // Restart the tick progress     
                NextManaTickProgress = 0;
            }
        }
        else {
            NextManaTickProgress += Time.deltaTime * ManaTickProgressPerSecond;
        }
    }

    private bool IncrementMana()
    {
        if (CurrentMana < MaxMana) {
            CurrentMana += 1;
            return true;
        }

        return false;
    }
    private void RestockSpell()
    {
        if (!deck.Any()) {
            isRestockingSpell = false;
            return;
        }
        
        Debug.Log("Dequeuing!");
        
        var spell = deck[0];
        deck.Remove(spell);
        if (!spellSlots.TryAddSpell(spell)) {
            throw new Exception("Couldn't add spell");
        }
        
        isRestockingSpell = false;
    }

    private void ReloadDeck()
    {
        if (deck.Any()) {
            Debug.LogWarning("Cannot reload a deck that still has spells in it");
            isReloadingDeck = false;
            return;
        }
        
        deck = startingDeck.ToList();
        deck = deck.OrderBy(_ => Random.value).ToList();
        
        isReloadingDeck = false;
    }

    public void StopStudying()
    {
        isStudying = false;
    }

    public void UseSpell(int spellSlotIndex, Vector3 point)
    {
        var spell = spellSlots.GetSpellAt(spellSlotIndex);
        if (spell == null) {
            throw new IndexOutOfRangeException("invalid spell slot");
        }

        if (CurrentMana < spell.spellCost) {
            Debug.LogWarning("Not enough mana to cast spell");
            return;
        }
        
        CurrentMana -= spell.spellCost;
        GetComponent<Spellcasting>().StartSpellCast(spell, point);
        spellSlots.ClearSpellAt(spellSlotIndex);

        // if (obj.GetComponent<StudySpellCast>() != null) {
        //     isStudying = true;
        // }
    }

    internal class SpellSlots
    {
        private readonly IReadOnlyList<SpellSlot> spellSlots;

        public SpellSlots(int spellCount)
        {
            var slots = new List<SpellSlot>();
            for (int i = 0; i < spellCount; i++) {
                slots.Add(new SpellSlot()); 
            }

            spellSlots = slots.AsReadOnly();
        }

        public bool TryAddSpell(SpellRecord spell)
        {
            foreach (var slot in spellSlots) {
                if (slot.Entry == null) {
                    slot.Entry = spell;
                    return true;
                }
            }

            return false;
        }

        public bool HasEmptySlot()
        {
            foreach (var slot in spellSlots) {
                if (slot.Entry == null) {
                    return true;
                }
            }

            return false;
        }

        public bool HasSpell(int i)
        {
            var spell = GetSpellAt(i);
            if (spell == null)
                return false;
            return true;
        }

        public bool HasAny()
        {
            foreach (var slot in spellSlots) {
                if (slot.Entry != null) {
                    return true;
                }
            }

            return false;
        }

        public void ClearSpellAt(int i)
        {
            spellSlots[i].Entry = null;
        }

        [CanBeNull]
        public SpellRecord GetSpellAt(int i)
        {
            return spellSlots.ElementAtOrDefault(i)?.Entry;
        }

        private class SpellSlot
        {
            [CanBeNull] public SpellRecord Entry = null;
        }
    } 
}