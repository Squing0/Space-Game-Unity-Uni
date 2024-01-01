using TMPro;
using UnityEngine;

namespace UI
{
    public class Charge : MonoBehaviour
    {
        public TMP_Text chargeText;
        private float chargeValue = 1;
        private bool chargeOn = true;
        public float chargeSpeeder;

        public GameObject win;
        private GameWinScreen winScreen;    // Put in UI manager? (fine either way)
        public bool ChargeOn
        {get { return chargeOn; } set { chargeOn = value;}}
        public float ChargeValue
        {get { return chargeValue; } set { chargeValue = value;}}
        public float ChargeSpeeder
        {get { return chargeSpeeder; } set { chargeSpeeder = value; }}
       
        private void Start()
        {
            winScreen = win.GetComponent<GameWinScreen>();
        }
        private void Update()
        {
            if (chargeValue < 100)
            {
                if (chargeOn)
                {
                    chargeValue += Time.deltaTime * chargeSpeeder;
                    chargeText.text = $"Charge: {(int)chargeValue}%";   
                }
            }
            else
            {
                winScreen.ActivateWin();
            }
        }
    }
}