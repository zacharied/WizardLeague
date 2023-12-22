using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillScreenUI : MonoBehaviour
{
    public List<SpellRecord> spells;
    public VisualTreeAsset listItem;
    private ListView listView;
    
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        listView = uiDocument.rootVisualElement.Q<ListView>("ListView");
        
        listView.itemsSource = spells;
        listView.makeItem = () => listItem.Instantiate();

        listView.bindItem = (e, i) =>
        {
            var item = spells[i];
            var icon = e.Q("SpellIcon");
            icon.style.backgroundImage = new StyleBackground(item.sprite);
        };
    }
}