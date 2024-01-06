using System.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public float aliveTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);    // Make so destroys once hit anything, not specifically enemy               
        }

        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Rock"))
        {
            Destroy(gameObject);
        }
        //Have code for just destroying object in general ?

        //Destroy(gameObject);  // For some reason, changing it to just this caused problems
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    public IEnumerator deleteProjectile(GameObject obj)
    {
        yield return new WaitForSeconds(aliveTime);

        Destroy(obj);
    }
}