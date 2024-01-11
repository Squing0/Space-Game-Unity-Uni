using System.Collections;
using UnityEngine;

// Manages methods related to bullet and rock projectiles.
public class ProjectileManager : MonoBehaviour
{
    // Time bullet stays alive if no triggers or collisions occur.
    public float aliveTime;

    private void OnTriggerEnter(Collider other)
    {
        // Destroys bullet if hits enemy.
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);         
        }

        // Destroys rock if hits player.
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Rock"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // If rock or bullet haven't hit target and collider with terrain or other objects, destroy.
        Destroy(gameObject);
    }
    // Deletes projectile if no triggers or collisions occur.
    public IEnumerator deleteProjectile(GameObject obj)
    {
        yield return new WaitForSeconds(aliveTime); // Speciifed time for projectile to stay alive.

        Destroy(obj);
    }
}