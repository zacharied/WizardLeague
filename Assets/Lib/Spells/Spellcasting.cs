using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lib
{
    public class Spellcasting : NetworkBehaviour
    {
        private List<GameObject> activeSpellCharges = new();
        private List<GameObject> activeSpellCasts = new();
        
        private void Update()
        {
            foreach (var spellObject in activeSpellCharges.ToList())
            {
                // Wait for spell charge to finish to cast spell
                if (spellObject.GetComponentsInChildren<ISpellChargeable>().All(c => c.IsDone())) {
                    Debug.Log("Spell charge finished");
                    foreach (var spellCastable in spellObject.GetComponents<ISpellCastable>())
                    {
                        spellCastable.SpellCast();
                    }
                    activeSpellCharges.Remove(spellObject);
                    activeSpellCasts.Add(spellObject);
                }
            }
            
            foreach (var spellObject in activeSpellCasts.ToList())
            {
                if (spellObject == null)
                    continue;
                
                if (spellObject.GetComponentsInChildren<ISpellFinishCondition>().All(c => c.ShouldFinish()))
                {
                    activeSpellCasts.Remove(spellObject);
                    GameObject.Destroy(spellObject);
                } 
            }
        }
        public static bool SpellIsDoneCharging(GameObject spellObject)
        {
            var done = true;
            foreach (var chargeable in spellObject.GetComponents<ISpellChargeable>()) {
                if (!chargeable.IsDone())
                    done = false;
            }
            return done;
        }

        public void StartSpellCast(SpellRecord spellRecord, Vector3 position)
        {
            // TODO: check if online
            Debug.Log("Starting spell cast");

            var spellName = spellRecord.spellPrefab.name;

            if (!IsHost) {
                StartSpellCast_ServerRpc(spellName, position);
                
                var spell = Instantiate(LoadSpell(spellName), position, Quaternion.identity);
                StartSpellCast_Inner(spell); 
            }
            else {
                StartSpellCast_ClientRpc(spellName, position);
            }
        }

        [ClientRpc(Delivery =  RpcDelivery.Reliable)]
        private void StartSpellCast_ClientRpc(string spellName, Vector3 position)
        {
            var spell = Instantiate(LoadSpell(spellName), position, Quaternion.identity);
            StartSpellCast_Inner(spell); 
            // TODO study mechanic
        }

        [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
        private void StartSpellCast_ServerRpc(string spellName, Vector3 position)
        {
            var spell = Instantiate(LoadSpell(spellName), position, Quaternion.identity);
            StartSpellCast_Inner(spell); 
            // TODO study mechanic
        }
        
        private void StartSpellCast_Inner(GameObject spellObject)
        {
            foreach (var c in spellObject.GetComponents<ISpellChargeable>())
            {
                c.ChargeSpell();
            }

            activeSpellCharges.Add(spellObject);
        }

        private GameObject LoadSpell(string spellName)
        {
            Debug.Log($"Loading spell record {spellName}");
            var spell = Resources.Load<SpellRecord>($"Spells/{spellName}/{spellName}");
            return spell.spellPrefab;
        }
    }
}