using UnityEngine;

namespace Lib.SpellFinishConditions
{
    public class ObjectNotDestroyedFinishCondition : MonoBehaviour, ISpellFinishCondition
    {
        public GameObject TrackedObject = null!;
        
        public bool ShouldFinish()
        {
            return TrackedObject == null;
        }
    }
}