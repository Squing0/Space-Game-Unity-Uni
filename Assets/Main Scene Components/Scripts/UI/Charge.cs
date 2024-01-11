using TMPro;
using UnityEngine;

namespace UI
{
    // Handles all methods relating to charge UI element.
    public class Charge : MonoBehaviour
    {
        // Charge text to change.
        public TMP_Text chargeText;

        // Initial charge values
        private float chargeValue = 1;  
        private float chargeSpeeder;   

        // Properties used as variables are used by other classes.
        public float ChargeValue
        {get { return chargeValue; } set { chargeValue = value;}}
        public float ChargeSpeeder
        {get { return chargeSpeeder; } set { chargeSpeeder = value; }}
       
        private void Update()
        {
            if (chargeValue < 100)  // Tracks charge going up to 100.
            {
                chargeValue += Time.deltaTime * chargeSpeeder;  // charge speeder allows speed of time to be controlled.
                chargeText.text = $"Charge: {(int)chargeValue}%";   // int used so that player sees whole values in UI.
            }
            else
            {
                UiManager.instance.ActivateWin();   // Win screen shown when charge hits 100.
            }
        }
    }
}