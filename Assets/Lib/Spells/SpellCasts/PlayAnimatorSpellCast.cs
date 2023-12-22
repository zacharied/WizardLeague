using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimatorSpellCast : MonoBehaviour, ISpellCastable
{
    public Animator animator;
    
    void Start()
    {
        animator.enabled = false;
    }

    public void SpellCast()
    {
        animator.enabled = true;
    }
}
