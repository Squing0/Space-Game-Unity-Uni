using System.Collections;
using TMPro;
using UnityEngine;

namespace Player
{
    public class GunShoot : MonoBehaviour
    {
        [Header("Shooting")]
        public Transform shootPos;
        public float bulletVelocity;
        public GameObject bullet;

        public float timeBetweenAttacks;

        public ParticleSystem gunSmoke;
        public UnityEngine.UI.Image enemyCrosshair;

        private bool readyToAttack;
        private ProjectileManager pm;
        private Camera m_Camera;

        public float magazineTotal, bulletTotal, reloadTime;    
        private float currentBullets;
        public TMP_Text reloadText;

        // Current Bullets property
        public float CurrentBullets
        { get { return currentBullets; } private set {}}

        private void Awake()
        {
            m_Camera = Camera.main;
            enemyCrosshair.enabled = false;

            readyToAttack = true;
            currentBullets = magazineTotal;
        }
        private void Start()
        {
            pm = bullet.GetComponent<ProjectileManager>();
            gunSmoke.Stop();
        }
        private void Update()
        {
            MyInput();
            MouseDetection();

            reloadText.text = $"{currentBullets}   {bulletTotal}";
        }

        private void MouseDetection()
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
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

        private void MyInput()
        {
            if (Input.GetMouseButton(0) && readyToAttack && currentBullets > 0)
            {
                Shoot();
            }

            if (Input.GetKey(KeyCode.R) && currentBullets < magazineTotal && bulletTotal > 0)
            {
                AudioManager.instance.reloadSound.Play();
                Invoke(nameof(Reload), reloadTime); // CHANGE TO COROUTINE FOR BOTH, BE CONSISTENT
            }

            //if (currentBullets == 0) // Would prefer to be -1 but this kinda works
            //{
            //    Invoke(nameof(Reload), reloadTime); // Make so takes longer, if run out of bullets and don't manually reload
            //}

            //if(bulletTotal == 0)
            //{
            //    readyToAttack = false;
            //}
        }

        private void Reload()
        {
            //bulletTotal -= magazineTotal - currentBullets;

            //if(bulletTotal < 0)
            //{
            //    currentBullets += bulletTotal;
            //    bulletTotal = 0;
            //}

            //if(bulletTotal > 0) 
            //{
            //    currentBullets = magazineTotal;
            //}
           
            float bulletsNeeded = magazineTotal - currentBullets;   //Chatgpt gave this, change?

            if (bulletTotal >= bulletsNeeded)
            {
                currentBullets = magazineTotal;
                bulletTotal -= bulletsNeeded;
            }
            else
            {
                currentBullets += bulletTotal;
                bulletTotal = 0;
            }
        }

        public void AddAmmo(int ammo)
        {
            if (currentBullets == magazineTotal)    
            {
                bulletTotal += ammo;
            }
            else
            {
                currentBullets += ammo;

                if (currentBullets > magazineTotal)
                {
                    float extraBullets = currentBullets - magazineTotal;
                    bulletTotal += extraBullets;

                    currentBullets = magazineTotal;
                }
            }
        }

        private void Shoot()
        {
            AudioManager.instance.shootSound.Play();

            readyToAttack = false;
            Vector3 mousePos = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePos);  //Unity tutorial used here

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

            }

            Vector3 alteredPosition = new Vector3(shootPos.transform.position.x, shootPos.transform.position.y, shootPos.transform.position.z);

            GameObject clone = Instantiate(bullet, alteredPosition, Quaternion.Euler(90, 0, 0));
            Rigidbody bulletRigidbody = clone.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = shootPos.transform.forward * bulletVelocity; // May be better to have more accurate/complex way of shooting here

            //bulletRigidbody.AddForce(shootPos.transform.forward * 20f, ForceMode.Impulse);    // Alternate way of shooting bullets!
            //bulletRigidbody.AddForce(shootPos.transform.up * 10f, ForceMode.Impulse);

            gunSmoke.Play();          

            StartCoroutine(pm.deleteProjectile(clone));
            StartCoroutine(ResetShooting());

            currentBullets--;
        }

        private IEnumerator ResetShooting()
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            readyToAttack = true;
        }
        private void Reset()
        {
            readyToAttack = true;
        }
    }
}