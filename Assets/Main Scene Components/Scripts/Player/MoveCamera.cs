using UnityEngine;

namespace Player
{
    // Moves camera according to player position.
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;
        private void Update()
        {
            transform.position = cameraPosition.position;   // Keeps camera attatched to player.
        }
    }
}
