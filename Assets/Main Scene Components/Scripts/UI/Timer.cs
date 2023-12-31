using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public float totalTime = 1;
        public bool timerOn;
        public TMP_Text timerText;
        private float charge = 1;
        private bool chargeOn = true;

        public bool ChargeOn
        {
            get { return chargeOn; }
            set { chargeOn = value; }
        }

        public float Charge
        {
            get { return charge; }
            set { charge = value; }
        }
        public float timeSpeeder;
        public float TimeSpeeder
        {
            get { return timeSpeeder; }
            set { timeSpeeder = value; }
        }

        //public GameObject gameover;
        //GameOverScreen gm;

        public GameObject win;
        GameWinScreen winScreen;

        // CHANGE SOME OF THIS OVER TO UI MANAGER

        private void Start()
        {
            timerOn = true;
            winScreen = win.GetComponent<GameWinScreen>();
        }
        private void Update()
        {
            if (charge < 100)
            {
                if (chargeOn)
                {
                    charge += Time.deltaTime * timeSpeeder;
                    timerText.text = $"Charge: {(int)charge}%";
                }
            }
            else
            {
                winScreen.ActivateWin();
            }
        }
    }
}