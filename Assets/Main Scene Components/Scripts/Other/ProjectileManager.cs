using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);    // Make so destroys once hit anything, not specifically enemy               
        }

        // Destroy(gameObject);
    }

    public IEnumerator deleteProjectile(GameObject obj)
    {
        yield return new WaitForSeconds(2f);

        Destroy(obj);
    }
}
