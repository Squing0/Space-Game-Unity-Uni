using UnityEngine;

namespace UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        public Camera m_camera;  // CHANGE NAME   // Try to see if can remove as is just one method

        private void Start()
        {
            m_camera = Camera.main;
        }
        private void Update()
        {
            transform.rotation = m_camera.transform.rotation; // CHECK WORKING DEPENDING ON FINAL PLAYER MODEL USED!
        }
    }
}
