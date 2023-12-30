using UnityEngine;

namespace Player
{
    public class PlayerBarrier : MonoBehaviour
    {
        public float distanceFromPlayer;

        private void Start()
        {
            distanceFromPlayer = 2f;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Vector3 direction = other.transform.position - transform.position;
                direction.y = 0;

                if(direction.magnitude < distanceFromPlayer)
                {
                    other.transform.position = transform.position + direction.normalized * distanceFromPlayer;
                }
            }
        }
    }
}