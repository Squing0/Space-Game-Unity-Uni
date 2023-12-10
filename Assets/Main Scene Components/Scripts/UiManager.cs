using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header ("Text elements")]
    public TMP_Text playerHealth;
    public TMP_Text shipHealth;

    public GameObject player;
    PlayerMovement pm;

    public GameObject ship;
    ShipManager sm;

    private void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
        sm = ship.GetComponent<ShipManager>();
    }
    private void Update()
    {
        playerHealth.text = $"Player Health: {pm.Health}";
        shipHealth.text = $"Ship Health: {sm.Health}";
    }   
}
    