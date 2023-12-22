using UnityEngine;

namespace Lib
{
    [AddComponentMenu("BallBoys/ParticleSystemAwaiter")]
    public class ParticleSystemAwaiter : MonoBehaviour
    {
        public bool IsDone()
        {
            return !gameObject.GetComponent<ParticleSystem>().IsAlive();
        } 
    }
}