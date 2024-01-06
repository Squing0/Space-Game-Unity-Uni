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

        public GameObject UIManager;
        private UiManager ui;
        public bool ChargeOn
        {get { return chargeOn; } set { chargeOn = value;}} // Make all properties like this
        public float ChargeValue
        {get { return chargeValue; } set { chargeValue = value;}}
        public float ChargeSpeeder
        {get { return chargeSpeeder; } set { chargeSpeeder = value; }}
       
        private void Start()
        {
            ui = UIManager.GetComponent<UiManager>();
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
                ui.ActivateWin();
            }
        }
    }
}