using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class FieldManager : MonoBehaviour
{
    public GameObject fieldPlayerObject = null;

    public List<SpellRecord> defaultSpellSet = new();
    
    void Start()
    {
        if (FindObjectOfType<FieldPlayer>() == null) {
            Debug.Log("Creating default FieldPlayer");
            var player = Instantiate(fieldPlayerObject);
            player.GetComponent<FieldPlayer>().startingDeck = defaultSpellSet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
