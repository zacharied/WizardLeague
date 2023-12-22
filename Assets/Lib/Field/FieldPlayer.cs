using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Lib;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;
using Random = UnityEngine.Random;

public class FieldPlayer : MonoBehaviour
{
    public Material InputAllowedMaterial = null!;
    public Material InputDisabledMaterial = null!;
    public GameObject pointerObject = null!;

    private GameObject pointer;

    public int playerNumber = 1;

    internal int selectedSpellIndex { get; private set; } = -1;

    [CanBeNull] private ISpellTarget activeReticule = null;
    private bool mouseJustHoveredField = true;
    private bool clearTargetReticule = true;

    internal PlayerStatus status;

    internal IEnumerable<SpellRecord> startingDeck = null;
    
    void Awake()
    {
        status = FindObjectsOfType<PlayerStatus>().SingleOrDefault(f => f.playerNumber == playerNumber);
        if (status == null) {
            Debug.Log("Creating inline status");
            status = gameObject.AddComponent<PlayerStatus>();
            status.playerNumber = playerNumber;
        }

        pointer = Instantiate(pointerObject);

        var networkedPlayer = FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None)
            .Single(p => p.IsLocalPlayer);
        networkedPlayer.SetFieldSceneLoaded();
    }

    void Start()
    {
        if (startingDeck != null) {
            status.startingDeck = startingDeck.ToList();
        }
        
        UniTask.Void(async () =>
        {
            await UniTask.WaitUntil(() => FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None).All(p => p.hasLoadedFieldScene));
            var player = FindObjectsByType<NetworkedPlayer>(FindObjectsSortMode.None)
                .Single(p => p.IsLocalPlayer);
            player.SynchronizeGameStart();
        });
    }

    // Update is called once per frame
    void Update()
    {
        // Clear reticule if necessary
        if (clearTargetReticule && activeReticule != null) {
            DeactivateReticule();
            clearTargetReticule = false;
        }
        
        // Cursor stuff
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, Camera.main.farClipPlane, 1 << 3).OrderByDescending(h => h.point.y).ToList();
        
        // Spells
        if (Input_HandleSpellKeys()) {
        }
        
        if (hits.Any())
        {
            pointer.transform.position = hits.First().point;
            if (hits.Any(c => c.collider.CompareTag("FieldTile")))
            {
                // The user is hovering over the field
                if (mouseJustHoveredField) {
                    if (selectedSpellIndex >= 0) {
                        ActivateSpellReticule();
                    }
                    
                    mouseJustHoveredField = false;
                }
                
                pointer.GetComponentInChildren<MeshRenderer>().material = InputAllowedMaterial;
                Input_HandleSpellPress(hits.First());
                
                clearTargetReticule = false;
            }
            else
            {
                pointer.GetComponentInChildren<MeshRenderer>().material = InputDisabledMaterial;
                
                clearTargetReticule = true;
                mouseJustHoveredField = true;
            }
        }
        
    }

    bool Input_HandleSpellKeys()
    {
        var keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
        foreach (var (key, i) in keys.Select((k, i) => (k, i))) {
            if (Input.GetKeyDown(key)) {
                if (selectedSpellIndex == i) {
                    // Deselect the spell
                    selectedSpellIndex = -1;
                    DeactivateReticule();
                }
                else {
                    selectedSpellIndex = i;
                    ActivateSpellReticule();
                }

                return true;
            }
        }

        return false;
    }

    void Input_HandleSpellPress(RaycastHit mouseRaycastCollision)
    {
        if (selectedSpellIndex >= 0 && Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) {
            var spell = status.spellSlots.GetSpellAt(selectedSpellIndex);
            if (spell == null) {
                return;
            }
            
            if (status.CurrentMana >= spell.spellCost) {
                // Create the spell object
                status.UseSpell(selectedSpellIndex, mouseRaycastCollision.point);
                selectedSpellIndex = -1;
                
                DeactivateReticule();
            }
            else {
                Debug.LogWarning("Attempted to cast a spell without enough mana");
            }
        }
    }

    private void ActivateSpellReticule()
    {
        if (activeReticule != null) {
            DeactivateReticule();
        }
        
        if (status.spellSlots.GetSpellAt(selectedSpellIndex) != null) {
            activeReticule = status.spellSlots.GetSpellAt(selectedSpellIndex).spellPrefab.GetComponent<ISpellTarget>();
            if (activeReticule == null) {
                Debug.LogWarning("No target found for spell!");
                return;
            }
            activeReticule!.DrawTarget();
        }
        else {
            Debug.LogWarning("Attempted to activate reticule with a reticule already active!");
        }
    }
    
    private void DeactivateReticule()
    {
        if (activeReticule != null) {
            activeReticule!.ClearTarget();
            activeReticule = null;
        }
    }

}