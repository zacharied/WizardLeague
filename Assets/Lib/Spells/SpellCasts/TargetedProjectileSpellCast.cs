using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetedProjectileSpellCast : MonoBehaviour, ISpellCastable
{
    /// <summary>
    /// Requires a RigidBody
    /// </summary>
    public GameObject projectilePrefab = null!;

    public float velocity = 100f;

    public void SpellCast()
    {
        var spawnPoint = GameObject.FindGameObjectWithTag("ProjectileSpawn_P1");
        var target = GameObject.FindGameObjectWithTag("GameBall");
        var projectile = Instantiate(projectilePrefab, gameObject.transform);
        var direction = target.transform.position - spawnPoint.transform.position;
        
        projectile.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

        projectile.transform.position = spawnPoint.transform.position;
        projectile.GetComponent<Rigidbody>().velocity = direction * velocity;
    }
}
