using System.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);    // Make so destroys once hit anything, not specifically enemy               
        }

        if(other.gameObject.CompareTag("Player") && gameObject.CompareTag("Rock"))
        {
            Destroy(gameObject);
        }
        // Have code for just destroying object in general?
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    public IEnumerator deleteProjectile(GameObject obj)
    {
        yield return new WaitForSeconds(2f);

        Destroy(obj);
    }
}