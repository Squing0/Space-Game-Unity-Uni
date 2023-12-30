using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonRestart : MonoBehaviour
    {
        public Button restart;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))    // Allows the user to restart with R as mouse can't be used due to it being used with the camera
            {
                restart.onClick.Invoke();
            }
        }
    }
}
