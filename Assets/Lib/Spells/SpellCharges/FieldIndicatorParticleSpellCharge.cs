using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace Lib
{
    /// <summary>
    /// A spell charge animation that plays particle effects on the field.
    /// </summary>
    public class FieldIndicatorParticleSpellCharge : MonoBehaviour, ISpellChargeable
    {
        public GameObject ParticleSystemsContainer;
        [CanBeNull] public GameObject Spinner;

        private bool isPlaying = false;
        private float duration = 0f;
        private float spinTimer = 0f;

        private void Start()
        {
            if (ParticleSystemsContainer == null)
            {
                throw new UnityException("no effect root");
            }

            duration = GetComponent<WaitDurationSpellCharge>().duration;
        }

        private void Update()
        {
            spinTimer += Time.deltaTime;

            if (Spinner != null) {
                Spinner.transform.eulerAngles = new Vector3(transform.eulerAngles.x, (duration / Mathf.Pow(spinTimer, 2)) * 15,
                    transform.eulerAngles.z);
            }
        }

        public void ChargeSpell()
        {
            isPlaying = true;
            foreach (var p in ParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>())
            {
                p.Play();
            }
        }

        public bool IsDone()
        {
            return true;
        }
    }

}