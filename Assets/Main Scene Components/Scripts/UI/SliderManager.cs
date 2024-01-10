using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderManager : MonoBehaviour
    {
        public Slider slider;
        private void Start()
        {
            if(gameObject.name != "MoralitySlider")
            {
                slider.value = 1;
            }
        }

        public void UpdateHealth(float health, float maxHealth)  
        {
            slider.value = health / maxHealth;
        }

        public void IncreaseMorality(float morality)
        {
            slider.value += morality;
        }

        public void DecreaseMorality(float morality)
        {
            slider.value -= morality;
        }
    }
}