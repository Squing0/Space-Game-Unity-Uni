using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ProjectileGunTutorial : MonoBehaviour
{
    //Bullet
    public GameObject bullet;

    // Bullet Force
    public float shootForce, upwardForce;

    // gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    public int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // Camera
    public Camera fpsCam;
    public Transform attackPoint;

    // Graphics
    //public GameObject muzzleFlash;
    public TMP_Text reloadText;

    // bug fixing :)
    public bool allowInvoke = true;

    private void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        reloadText.SetText($"{bulletsLeft / bulletsPerTap}/{magazineSize / bulletsPerTap}");
    }

    private void MyInput()
    {
        // Check if allowed to hold fire button
        if(allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);

        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.E) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        // Reload automatically when no bullets
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        // Shooting
        if(readyToShoot && shooting && !reloading & bulletsLeft > 0)
        {
            // Set bullet shots to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Find the exact posisiton using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75); // Just a point far away from player
        }

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Calculate Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);    // just add spread to last di

        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        // Rotate bullet to shoot tutorial
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // Adding forces to bullet
        //currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().velocity = directionWithoutSpread.normalized * shootForce;
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse); // Good for bouncing projectiles like grenades

        // Delete Bullet
        //StartCoroutine(DeleteBullet(currentBullet));

        // Instantiate muzzle flash, if you have one
        //Instantiate (muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }      

        // If more than one bulletsPerTap make sure to repeat shoot function
        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShots);
        }
    }

    private IEnumerator DeleteBullet(GameObject clone)
    {
        Destroy(clone);
        yield return new WaitForSeconds(5f);
    }

    private void ResetShot() 
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished() 
    { 
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
