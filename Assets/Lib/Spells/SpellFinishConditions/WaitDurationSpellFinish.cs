using System.Collections;
using System.Collections.Generic;
using Lib;
using UnityEngine;

public class WaitDurationSpellFinish : MonoBehaviour, ISpellFinishCondition
{
    public float duration = 0.5f;
    
    private bool done = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Finish), duration);     
    }

    private void Finish()
    {
        done = true;
    }

    public bool ShouldFinish() => done;
}
