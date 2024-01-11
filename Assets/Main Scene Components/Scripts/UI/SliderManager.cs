using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // Manages health and morality slider.
    public class SliderManager : MonoBehaviour
    {
        // Slider to be manipulated.
        public Slider slider;
        private void Start()
        {
            if(gameObject.name != "MoralitySlider") // Morality slider will be set to 0.5, this is for health
            {
                slider.value = 1;   
            }
        }

        // Health slider value is updated based on current and max health.
        public void UpdateHealth(float health, float maxHealth)  
        {
            slider.value = health / maxHealth;
        }

        // Morality slider value is either increased or decreased.
        public void ChangeMorality(float morality)
        {
            slider.value += morality;
        }
    }
}