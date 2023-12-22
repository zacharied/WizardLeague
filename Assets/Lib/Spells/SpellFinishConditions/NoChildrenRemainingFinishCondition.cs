using System;
using UnityEngine;

namespace Lib.SpellFinishConditions
{
    public class NoChildrenRemainingFinishCondition : MonoBehaviour, ISpellFinishCondition
    {
        public GameObject root = null!;

        private void Start()
        {
            if (root == null) root = gameObject;
        }

        public bool ShouldFinish()
        {
            return root.transform.childCount == 0;
        }
    }
}