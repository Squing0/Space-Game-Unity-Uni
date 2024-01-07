using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        //HealthBar hb;

        ////public float Health { get; private set; }
        ////public float MaxHealth { get; private set; }
        //public float health;
        //public float maxHealth;

        //public HealthBar(float health, float maxHealth)
        //{
        //    this.health = health;
        //    this.maxHealth = maxHealth;
        //}
        private void Start()
        {
            slider.value = 1;
        }
        //private void Awake()
        //{
        //    GameObject gameObject = new GameObject("Health Controller");
        //    hb = gameObject.AddComponent<HealthBar>();
        //}

        public void UpdateHealth(float health, float maxHealth)  // youtube tut
        {
            slider.value = health / maxHealth;
        }

        //public void UpdateHealth()  // youtube tut
        //{
        //    slider.value = health / maxHealth;
        //}

        //public void DecreaseHealth(int damage)
        //{
        //    health -= damage;
        //    UpdateHealth();
        //}

        //public void IncreaseHealth(int amount)
        //{
        //    health += amount;
        //    UpdateHealth();
        //}
    }
}