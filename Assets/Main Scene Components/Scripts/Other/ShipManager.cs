using UnityEngine;
using UI;
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
    private GameOverScreen gm;

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
            gm.ActivateGameover("Your ship was destroyed!");
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Knife"))       // Change to compare tag for this and other occurences
        {
            health--;
            healthBar.updateHealth(health, maxHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            health--;
            healthBar.updateHealth(health, maxHealth);
        }
    }
}
