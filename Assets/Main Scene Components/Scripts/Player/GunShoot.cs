using System.Collections;
using TMPro;
using UnityEngine;

namespace Player
{
    // Handles all variables and methods relating to player shooting gun.
    public class GunShoot : MonoBehaviour
    {
        [Header("Shooting")]
        public Transform shootPos;
        public GameObject bullet;
        public float bulletVelocity;
        public float timeBetweenAttacks;

        [Header("Effects")]
        public ParticleSystem gunSmoke;
        public UnityEngine.UI.Image enemyCrosshair;
        public TMP_Text reloadText;

        [Header("Reload")]
        public float magazineTotal;
        public float bulletTotal;
        public float reloadTime;

        // Script to manipulate.
        private ProjectileManager projectileManager;

        private Camera mCamera;
        private bool readyToAttack;

        private float currentBullets;
        // Property used as variable used by other scripts.
        public float CurrentBullets
        { get { return currentBullets; } private set {}}

        private void Awake()
        {
            // Uses main camera in scene and initially sets enemy cross hair to false.
            mCamera = Camera.main;
            enemyCrosshair.enabled = false;

            // Player is initially able to attack woth a full attack.
            readyToAttack = true;
            currentBullets = magazineTotal;
        }
        private void Start()
        {
            projectileManager = bullet.GetComponent<ProjectileManager>();
            gunSmoke.Stop();    // Animation is stopped when scene starts.
        }
        private void Update()
        {
            MouseDetection();
            reloadText.text = $"{currentBullets}   {bulletTotal}";
        }
        private void FixedUpdate()
        {
            MyInput();
        }

        // Detects if player is looking at enemy and shows crosshair if are.
        private void MouseDetection()
        {
            // Raycast used to detect if player sees enemy based on mouse position.
            Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    enemyCrosshair.enabled = true;
                }
                else
                {
                    enemyCrosshair.enabled = false;
                }
            }
        }
        // Calls method based on player inputs and conditions.
        private void MyInput()
        {
            // If player clicks left mouse and has no bullets, can attack.
            if (Input.GetMouseButton(0) && readyToAttack && currentBullets > 0)
            {
                Shoot();
            }

            // Reloads with R, so long as magazine isn't full and there are bullets available.
            if (Input.GetKey(KeyCode.R) && currentBullets < magazineTotal && bulletTotal > 0)
            {
                AudioManager.instance.reloadSound.Play();
                StartCoroutine(Reload());   // Coroutine used so player can't spam reload.
            }
        }
        // Reloads player's bullets.
        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(reloadTime);

            // Calculates required ammo based on player's current bullets.
            float requiredAmmo = magazineTotal - currentBullets;   

            // Ammo taken from bullet toal and current bullets filled if there are more total bullets than required.
            if (bulletTotal >= requiredAmmo)
            {
                bulletTotal -= requiredAmmo;
                currentBullets = magazineTotal;
            }
            else    // If not more bullets than required, current bullets given remainder and total bullets emptied.
            {
                currentBullets += bulletTotal;
                bulletTotal = 0;
            }
        }
        // Adds ammo when powerup gotten.
        public void AddAmmo(int ammo)
        {
            // If player's current bullets are full, simply add to total.
            if (currentBullets == magazineTotal)    
            {
                bulletTotal += ammo;
            }
            else
            {
                // Add ammo given to current bullets.
                currentBullets += ammo;

                // If ammo goes over magazine total.
                if (currentBullets > magazineTotal)
                {
                    // Calculate specific number of extra bullets and add to bullet total.
                    float extraBullets = currentBullets - magazineTotal;
                    bulletTotal += extraBullets;

                    // Reset current bullets to magazine total.
                    currentBullets = magazineTotal;
                }
            }
        }
        // Shoots bullet projectile from player gun.
        private void Shoot()
        {
            // Plays sound effect and particle effect.
            AudioManager.instance.shootSound.Play();
            gunSmoke.Play();

            // Instantiates bullet at specific rotation and shoots at player at specific velocity.
            Rigidbody rb = Instantiate(bullet, shootPos.transform.position, Quaternion.Euler(90, 0, 0))
                .GetComponent<Rigidbody>();
            rb.velocity = shootPos.transform.forward * bulletVelocity;

            readyToAttack = false;

            StartCoroutine(projectileManager.deleteProjectile(rb.gameObject));  // Deletes projectile if not triggered or collided with objects.
            StartCoroutine(ResetShooting());    // Resets shooting so player can't spam wit readyToAttack bool.

            currentBullets--;   // Takes one bullet off.
        }
        // Resets shooting with specified time.
        private IEnumerator ResetShooting()
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            readyToAttack = true;
        }
    }
}