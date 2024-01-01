using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider.value = 1;
        }

        public void updateHealth(float health, float maxHealth)  // youtube tut
        {
            slider.value = health / maxHealth;
        }
    }
}