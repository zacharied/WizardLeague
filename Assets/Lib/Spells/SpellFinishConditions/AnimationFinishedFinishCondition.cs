using System.Collections;
using System.Collections.Generic;
using Lib;
using UnityEngine;

public class AnimationFinishedFinishCondition : MonoBehaviour, ISpellFinishCondition
{
    public AnimationClip clip;

    private bool shouldFinish = false; 
    
    // Start is called before the first frame update
    void Awake()
    {
        clip.AddEvent(new AnimationEvent()
        {
            time = clip.length,
            functionName = nameof(AnimationFinished)
        });
    }

    public void AnimationFinished()
    {
        shouldFinish = true;
    }

    public bool ShouldFinish() => shouldFinish;
}
