using UnityEngine;
using TMPro;
using Player;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        public GameObject playerObj;
        public GameObject timerObj;
        public GameObject shipObj;

        private PlayerMovement playerMovement;
        private Timer timer;
        private ShipManager shipManager; // CHANGE FOR THE LOVE OF GOD

        private float totalScore;   // change to int?
        private void Start()
        {
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            timer = timerObj.GetComponent<Timer>();
            shipManager = shipObj.GetComponent<ShipManager>();
        }

        public void CalculateScore(string endReason, TMP_Text scoreText)
        {
            timer.ChargeOn = false;
            
            totalScore = (100 - timer.Charge) + (playerMovement.Health * 25) + (shipManager.Health * 25);
            scoreText.text = $"{endReason}\nScore: {(int)totalScore}";
        }
    }
}