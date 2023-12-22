using UnityEngine;

namespace Lib.SpellCasts
{
    public class PlayAnimationSpellCast : MonoBehaviour, ISpellCastable
    {
        public Animation animation;
    
        void Start()
        {
            animation.enabled = false;
        }

        public void SpellCast()
        {
            animation.enabled = true;
        }
    }
}