using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Spell cast that applies a constant effect to the field.
/// </summary>
public class CreateObjectForDurationSpellCast : MonoBehaviour, ISpellCastable
{
    public float duration = 3f;
    public float delay = 0f;
    public GameObject Object = null;

    public void Start()
    {
        Debug.Log("Disabling components");
        Object.GetComponent<Collider>().enabled = false;
        Object.GetComponent<Renderer>().enabled = false;
    }

    private void OnValidate()
    {
        if (Object == null)
        {
            throw new UnityException();
        }
    }

    private IEnumerator StartCreateObjectDelay()
    {
        yield return new WaitForSeconds(delay);
        CreateObject();
    }

    public void CreateObject()
    {
        Object.GetComponent<Collider>().enabled = true;
        Object.GetComponent<Renderer>().enabled = true;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void SpellCast()
    {
        StartCoroutine(StartCreateObjectDelay());
    }
}