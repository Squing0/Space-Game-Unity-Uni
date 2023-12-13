using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header ("Text elements")]
    public TMP_Text playerHealth;
    public TMP_Text shipHealth; // Is this class needed now?

    private void Update()
    {
        playerHealth.text = $"Player Health:";
        shipHealth.text = $"Ship Health:";
    }
}
    