using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileGunTutorial : MonoBehaviour
{
    //Bullet
    public GameObject bullet;

    // Bullet Force
    public float shootForce, upwardForce;

    // gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    public int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // Camera
    public Camera fpsCam;
    public Transform attackPoint;

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

        bulletsLeft--;
        bulletsShot++;
    }
}
