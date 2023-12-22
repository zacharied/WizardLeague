using System;
using UnityEngine;

namespace Lib
{
    public class WaitDurationSpellCharge : MonoBehaviour, ISpellChargeable
    {
        public float duration = 3f;
        private bool isDone = false;
        
        public void ChargeSpell()
        {
            Invoke(nameof(SetDone), duration);
        }

        public bool IsDone()
        {
            return isDone;
        }

        private void SetDone()
        {
            isDone = true;
        }
    }
}