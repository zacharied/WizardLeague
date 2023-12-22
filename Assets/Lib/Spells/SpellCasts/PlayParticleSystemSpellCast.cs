using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Lib.SpellCasts
{
    public class PlayParticleSystemSpellCast : MonoBehaviour, ISpellCastable
    {
        public GameObject ParticleSystemsContainer;

        private void Start()
        {
            foreach (var particle in ParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>())
            {
                particle.Stop();
            }
        }

        public void SpellCast()
        {
            foreach (var particle in ParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>())
            {
                particle.Play();
            }
        }

        public bool ShouldFinish()
        {
            foreach (var particle in ParticleSystemsContainer.GetComponentsInChildren<ParticleSystem>())
            {
                if (particle.IsAlive())
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}