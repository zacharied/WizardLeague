using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lib.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenUI : MonoBehaviour
{
    public VisualTreeAsset SpellQueueItem; 
    
    private VisualElement rootElement;
    private FieldPlayer playerScript;
    private int lastSpellIndex = -2;
    
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        playerScript = FindObjectOfType<FieldPlayer>();
        
        PopulateSpellSlots();
        
        var list = rootElement.Q("SkillScreenUI").Q<ListView>("ListView");
        list.makeItem = () => SpellQueueItem.Instantiate();
        list.bindItem = (v, i) =>
        {
            var spell = playerScript.status.deck[playerScript.status.deck.Count - 1 - i];
            v.Q<Label>("SpellNameLabel").text = spell.spellName;
            v.Q("SpellIcon").style.backgroundImage = spell.sprite.texture;
        };

        rootElement.Q<Label>("ReloadingLabel").visible = false;
    }

    private void Update()
    {
        PopulateSpellSlots();
        UpdateSpellSlots();
        UpdateManaDisplay();
        UpdateSpellQueue();

        rootElement.Q<Label>("ReloadingLabel").visible = playerScript.status.isReloadingDeck;
    }

    private void UpdateManaDisplay()
    {
        var element = rootElement.Q("ManaInfo");
        var number = rootElement.Q<Label>("Label");
        number.text = playerScript.status.CurrentMana.ToString();
        
        var progressBar = element.Q<CircularProgressBar>("Progress");
        progressBar.progress = playerScript.status.NextManaTickProgress * 100;
    }

    void PopulateSpellSlots()
    {
        var player = FindObjectOfType<FieldPlayer>();
        if (player == null)
            return;
        
        foreach (var (spellSlotButton, i) in rootElement.Q("SpellSlotsUI").Query("SpellSlotButton").Build().Select((e, i) => (e, i))) {
            spellSlotButton.Q<Label>("SpellSlotNumberLabel").text = (i + 1).ToString();
            
            var spell = player.status.spellSlots.GetSpellAt(i);
            if (spell == null) {
                var button = spellSlotButton.Q<Button>("Button");
                button.style.backgroundImage = null;
                button.style.backgroundColor = Color.gray;
                spellSlotButton.Q<Button>("Button").text = string.Empty;
            }
            else {
                spellSlotButton.Q<Button>("Button").style.backgroundImage = new StyleBackground(spell.sprite);
                spellSlotButton.Q<Button>("Button").text = spell.spellCost.ToString();
            }
        }
    }

    private void UpdateSpellSlots()
    {
        foreach (var (spellSlotButton, i) in rootElement.Q("SpellSlotsUI").Query("SpellSlotButton").Build().Select((e, i) => (e, i))) {
            var button = spellSlotButton.Q<Button>("Button");
            if (playerScript.selectedSpellIndex == i) {
                spellSlotButton.AddToClassList("selected-spell");
            }
            else {
                spellSlotButton.RemoveFromClassList("selected-spell");
            }
        }

        lastSpellIndex = playerScript.selectedSpellIndex;
    }

    private void UpdateSpellQueue()
    {
        var list = rootElement.Q("SkillScreenUI").Q<ListView>("ListView");
        list.itemsSource = playerScript.status.deck.ToList();;
        list.Rebuild();
    }
}
