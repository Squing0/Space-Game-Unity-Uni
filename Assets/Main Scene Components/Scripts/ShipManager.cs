using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [Header("Health")]
    public int health = 20;

    public int Health
    {
        get { return health; }
        set { }
    }

    public GameObject gameover;
    GameOverScreen gm;

    private void Start()
    {
        gm = gameover.GetComponent<GameOverScreen>();
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
        }
    }
}
