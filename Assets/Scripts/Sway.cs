using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Non ADS Mouse Look Tilt")]
    [SerializeField] private float nonADSTiltIntensity;
    [SerializeField] private float nonADSTiltSmoothness;
    [SerializeField] private float nonADSMaxRotation;

    [Header("ADS Mouse Look Tilt")]
    [SerializeField] private float adsTiltIntensity;
    [SerializeField] private float adsTiltSmoothness;
    [SerializeField] private float adsMaxRotation;

    [Header("Mouse Look Tilt")]
    [SerializeField] private float tiltIntensity;
    [SerializeField] private float tiltSmoothness;
    [SerializeField] private float maxRotation;

    [Header("Breathe Rotation")]
    [SerializeField] private float breathTiltIntensity;
    [SerializeField] private float maxBreathRotation;

    [SerializeField] private bool xRotation = true;
    [SerializeField] private bool yRotation = true;
    [SerializeField] private bool zRotation = true;

    [SerializeField] private float period = 2f;

    [Header("Sway-Idle")]
    [SerializeField] private float idleSwayAmountA = 1;
    [SerializeField] private float idleSwayAmountB = 2;
    [SerializeField] private float idleSwayScale = 100;
    [SerializeField] private float idleSwayLerpSpeed = 14;
    [SerializeField] private float idleSwaySpeed = 1f;

    [Header("Sway-Walking")]
    [SerializeField] private float walkingSwayAmountA = 1;
    [SerializeField] private float walkingSwayAmountB = 2;
    [SerializeField] private float walkingSwayScale = 100;
    [SerializeField] private float walkingSwayLerpSpeed = 14;
    [SerializeField] private float walkingSwaySpeed = 1f;

    [Header("Sway-Running")]
    [SerializeField] private float runningSwayAmountA = 1;
    [SerializeField] private float runningSwayAmountB = 2;
    [SerializeField] private float runningSwayScale = 100;
    [SerializeField] private float runningSwayLerpSpeed = 14;
    [SerializeField] private float runningSwaySpeed = 1f;

    [Header("Sway-Aiming")]
    [SerializeField] private float aimingSwayAmountA = 1;
    [SerializeField] private float aimingSwayAmountB = 2;
    [SerializeField] private float aimingSwayScale = 100;
    [SerializeField] private float aimingSwayLerpSpeed = 14;
    [SerializeField] private float aimingSwaySpeed = 1f;

    [Header("Sway")]
    [SerializeField] private float swayAmountA = 1;
    [SerializeField] private float swayAmountB = 2;
    [SerializeField] private float swayScale = 100;
    [SerializeField] private float swayLerpSpeed = 14;
    [SerializeField] private float swaySpeed = 1f;

    [SerializeField] private float lerpDuration = 2f;

    private float swayTime;
    private Vector3 swayPosition;

    private Quaternion originRotation;

    const float tau = Mathf.PI * 2f;

    private void Start()
    {
        originRotation = transform.localRotation;
        PlayerMovement.PlayerStateChanged += HandlePlayerStateChanged;
        UpdateBreatheWeaponPosition();
        NonADSTiltWeapon();
    }

    private void OnDestroy()
    {
        PlayerMovement.PlayerStateChanged -= HandlePlayerStateChanged;
    }

    private void HandlePlayerStateChanged()
    {
        UpdateBreatheWeaponPosition();
    }

    private void Update()
    {
        TiltWeapon();
        WeaponSway();
        // UpdateBreatheWeaponPosition();
        TiltWeaponWhileBreathing();
    }

    #region Weapon Sway

    private void UpdateBreatheWeaponPosition()
    {
        switch (PlayerMovement.GetPlayerState)
        {
            case PlayerState.Idle:
                IdleWeaponSway();
                NonADSTiltWeapon();
                break;
            case PlayerState.Walking:
                WalkingWeaponSway();
                NonADSTiltWeapon();
                break;
            case PlayerState.Running:
                RunningWeaponSway();
                NonADSTiltWeapon();
                break;
            case PlayerState.Jumping:
                IdleWeaponSway();
                NonADSTiltWeapon();
                break;
            case PlayerState.Aiming:
                AimingWeaponSway();
                ADSTiltWeapon();
                break;
        }
    }

    private void WeaponSway()
    {
        Vector3 targetPosition = LissajousCurve(swayTime * swaySpeed, swayAmountA, swayAmountB) / swayScale;

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }

        transform.localPosition = swayPosition;
    }


    private void WalkingWeaponSway()
    {
        swayAmountA = walkingSwayAmountA;
        swayAmountB = walkingSwayAmountB;
        swayScale = walkingSwayScale;
        swayLerpSpeed = walkingSwayLerpSpeed;
        swaySpeed = walkingSwaySpeed;
    }

    private void IdleWeaponSway()
    {
        swayAmountA = idleSwayAmountA;
        swayAmountB = idleSwayAmountB;
        swayScale = idleSwayScale;
        swayLerpSpeed = idleSwayLerpSpeed;
        swaySpeed = idleSwaySpeed;
    }

    private void RunningWeaponSway()
    {
        swayAmountA = runningSwayAmountA;
        swayAmountB = runningSwayAmountB;
        swayScale = runningSwayScale;
        swayLerpSpeed = runningSwayLerpSpeed;
        swaySpeed = runningSwaySpeed;
    }

    private void AimingWeaponSway()
    {
        swayAmountA = aimingSwayAmountA;
        swayAmountB = aimingSwayAmountB;
        swayScale = aimingSwayScale;
        swayLerpSpeed = aimingSwayLerpSpeed;
        swaySpeed = aimingSwaySpeed;
    }

    private IEnumerator Lerp(float _swayAmountA, float _swayAmountB, float _swayScale, float _swayLerpSpeed, float _swaySpeed)
    {
        float timeElapsed = 0;
        float startSwayScale = swayScale;
        float startSwaySpeed = swaySpeed;
        while (timeElapsed < lerpDuration)
        {
            swayScale = Mathf.Lerp(startSwayScale, _swayScale, timeElapsed / lerpDuration);

            // swaySpeed = Mathf.Lerp(startSwaySpeed, _swaySpeed, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        swayScale = _swayScale;
    }

    private Vector3 LissajousCurve(float time, float a, float b)
    {
        return new Vector3(Mathf.Sin(time), a * Mathf.Sin(b * time + Mathf.PI));
    }

    #endregion

    #region TiltWeapon

    private void TiltWeaponWhileBreathing()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // continually growing over time


        float rawSineWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        float xTilt = Mathf.Clamp(rawSineWave * breathTiltIntensity, -maxBreathRotation, maxBreathRotation);
        float yTilt = Mathf.Clamp(rawSineWave * breathTiltIntensity, -maxBreathRotation, maxBreathRotation);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(
            0f,
            0f,
            zRotation ? -xTilt : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation * targetRotation, Time.deltaTime * tiltSmoothness);
    }

    private void TiltWeapon()
    {
        float xInput = Input.GetAxis("Mouse X");
        float yInput = Input.GetAxis("Mouse Y");

        float xTilt = Mathf.Clamp(xInput * tiltIntensity, -maxRotation, maxRotation);
        float yTilt = Mathf.Clamp(yInput * tiltIntensity, -maxRotation, maxRotation);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(
            xRotation ? yTilt : 0f,
            yRotation ? -xTilt : 0f,
            zRotation ? -xTilt : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation * targetRotation, Time.deltaTime * tiltSmoothness);
    }

    private void ADSTiltWeapon()
    {
        tiltIntensity = adsTiltIntensity;
        tiltSmoothness = adsTiltSmoothness;
        maxRotation = adsMaxRotation;
    }

    private void NonADSTiltWeapon()
    {
        tiltIntensity = nonADSTiltIntensity;
        tiltSmoothness = nonADSTiltSmoothness;
        maxRotation = nonADSMaxRotation;
    }

    #endregion
}
