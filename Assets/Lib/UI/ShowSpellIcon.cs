using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpellIcon : MonoBehaviour
{
    public int spellIndex = 0;

    public Sprite emptySlotSprite;

    internal bool isSelected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<FieldPlayer>();
        GetComponent<Image>().sprite = player.status.spellSlots.GetSpellAt(spellIndex)?.sprite ? player.status.spellSlots.GetSpellAt(spellIndex)?.sprite : emptySlotSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) {
            if (gameObject.GetComponent<Outline>() == null) {
                var outline = gameObject.AddComponent<Outline>();
                outline.effectColor = Color.black;
                outline.effectDistance = new Vector2(5, -5);
                outline.useGraphicAlpha = false;
            }
        }
        else {
            Destroy(gameObject.GetComponent<Outline>());
        }
    }
}
