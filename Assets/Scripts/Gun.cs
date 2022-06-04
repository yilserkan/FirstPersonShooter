using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private CameraRecoil recoil;
    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private Camera gunCamera;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask targetLayerMask;

    [SerializeField] private int damage;
    [SerializeField] private float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonHold;
    [SerializeField] private int bulletsLeft, bulletsShot;

    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Recoil")]
    [SerializeField] public float returnSpeed;
    [SerializeField] public float snappiness;

    [SerializeField] public float recoilX;
    [SerializeField] public float recoilY;
    [SerializeField] public float recoilZ;

    [SerializeField] public float aimRecoilX;
    [SerializeField] public float aimRecoilY;
    [SerializeField] public float aimRecoilZ;

    [SerializeField] private float shootSpeed;
    [SerializeField] private float gravityForce;
    [SerializeField] private float bulletLifeTime;

    private bool readyToFire, reloading, shooting;
    private RaycastHit rayHit;

    private void Start() 
    {
        readyToFire = true;
        bulletsLeft = magazineSize;    
    }

    private void Update() 
    {
        PlayerInputs();
    }

    private void PlayerInputs()
    {
        if(allowButtonHold)
        {
            shooting = Input.GetButton("Fire1"); 
        }
        else
        {
            shooting = Input.GetButtonDown("Fire1");
        }

        if(Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            Reload();
        }

        if(readyToFire && !reloading && shooting && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        if(PlayerMovement.GetPlayerState == PlayerState.Running) { return; }

        readyToFire = false;
        bulletsLeft--;
        bulletsShot--;

        recoil.RecoilFire();
        mouseLook.UpdateSensToShootingSens();

        // float x = UnityEngine.Random.Range(spread, -spread);
        // float y = UnityEngine.Random.Range(spread, -spread);

        Vector3 shootDirection = gunCamera.transform.forward ;

        // if(Physics.Raycast(gunCamera.transform.position, shootDirection, out rayHit, range, targetLayerMask))
        // {
        //     Debug.DrawRay(rayHit.point, rayHit.normal * 100f,Color.red, 100f);
        //     Instantiate(bulletHole, rayHit.point, Quaternion.LookRotation(rayHit.normal)); 
        //     Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //     Debug.Log(rayHit.normal);
        // }
        // Debug.DrawRay(attackPoint.position, attackPoint.forward * 100f, Color.red, 100f);
        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.LookRotation(attackPoint.forward));
        bullet.GetComponent<Bullet>().Initialize(attackPoint, shootSpeed, gravityForce);

        Destroy(bullet, bulletLifeTime);

        if(bulletsPerTap > 0 && bulletsShot > 0)
        {
            Shoot();
        }

        Invoke(nameof(StopShooting), timeBetweenShooting);
    }

    private void StopShooting()
    {
        readyToFire = true;
        mouseLook.UpdateSensToNonShootingSens();
    }

    private void Reload()
    {
        reloading = true;

        Invoke(nameof(StopReloading), reloadTime);
    }

    private void StopReloading()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
