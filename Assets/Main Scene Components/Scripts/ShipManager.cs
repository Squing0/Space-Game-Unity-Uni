using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{
    [Header("Health")]
    public int health = 20;
    public int maxHealth = 20;

    public int Health
    {
        get { return health; }
        set { }
    }

    public GameObject gameover;
    GameOverScreen gm;

    [SerializeField]
    private HealthBar healthBar;

    private void Start()
    {
        gm = gameover.GetComponent<GameOverScreen>();
        health = maxHealth;
    }
    private void Update()
    {
        if (health < 1)
        {
            gm.ActivateGameover(3, 3);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health--;
            healthBar.updateHealth(health, maxHealth);
        }
    }
}
