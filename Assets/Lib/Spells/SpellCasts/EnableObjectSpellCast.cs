using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectSpellCast : MonoBehaviour, ISpellCastable
{
    public GameObject subject;
    public float delay = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var c in subject.GetComponentsInChildren<Collider>()) {
            c.enabled = false;
        }
        foreach (var c in subject.GetComponentsInChildren<Renderer>()) {
            c.enabled = false;
        }
    }

    // Update is called once per frame
    void CreateObject()
    {
        foreach (var c in subject.GetComponentsInChildren<Collider>()) {
            c.enabled = true;
        }
        foreach (var c in subject.GetComponentsInChildren<Renderer>()) {
            c.enabled = true;
        }
    }
    
    private IEnumerator StartCreateObjectDelay()
    {
        yield return new WaitForSeconds(delay);
        CreateObject();
    }

    public void SpellCast()
    {
        StartCoroutine(StartCreateObjectDelay());
    }
}