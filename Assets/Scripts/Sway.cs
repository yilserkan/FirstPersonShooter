using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [Header("Sway - Scriptable Objects")]
    [SerializeField] private SwayScriptableObject idleSwayScriptableObject;
    [SerializeField] private SwayScriptableObject walkingSwayScriptableObject;
    [SerializeField] private SwayScriptableObject runningSwayScriptableObject;
    [SerializeField] private SwayScriptableObject aimingSwayScriptableObject;

    private SwayScriptableObject activeSwayScriptableObject;
    
    [Header("Sens - Scriptable Objects")]
    [SerializeField] private SensitivityScriptableObject nonAdsSensitivityScriptableObject;
    [SerializeField] private SensitivityScriptableObject adsSensitivityScriptableObject;

    private SensitivityScriptableObject activeSensitivityScriptableObject;
    
    [SerializeField] private bool xRotation = true;
    [SerializeField] private bool yRotation = true;
    [SerializeField] private bool zRotation = true;

    [SerializeField] private float period = 2f;
    
    private float swayTime;
    private Vector3 swayPosition;

    private Quaternion originRotation;

    const float tau = Mathf.PI * 2f;

    private void Start()
    {
        UpdateActiveSwayScriptableObject(idleSwayScriptableObject);
        UpdateActiveSensitivityScriptableObject(nonAdsSensitivityScriptableObject);
        
        originRotation = transform.localRotation;
        
        PlayerMovement.PlayerStateChanged += HandlePlayerStateChanged;
        
        UpdateBreatheWeaponPosition();
        
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
                UpdateActiveSwayScriptableObject(idleSwayScriptableObject);
                UpdateActiveSensitivityScriptableObject(nonAdsSensitivityScriptableObject);
                break;
            case PlayerState.Walking:
                UpdateActiveSwayScriptableObject(walkingSwayScriptableObject);
                UpdateActiveSensitivityScriptableObject(nonAdsSensitivityScriptableObject);
                break;
            case PlayerState.Running:
                UpdateActiveSwayScriptableObject(runningSwayScriptableObject);
                UpdateActiveSensitivityScriptableObject(nonAdsSensitivityScriptableObject);
                break;
            case PlayerState.Jumping:
                UpdateActiveSwayScriptableObject(idleSwayScriptableObject);
                UpdateActiveSensitivityScriptableObject(nonAdsSensitivityScriptableObject);
                break;
            case PlayerState.Aiming:
                UpdateActiveSwayScriptableObject(aimingSwayScriptableObject);
                UpdateActiveSensitivityScriptableObject(adsSensitivityScriptableObject);
                break;
        }
    }

    private void UpdateActiveSwayScriptableObject(SwayScriptableObject swayScriptableObject)
    {
        activeSwayScriptableObject = swayScriptableObject;
    }

    private void UpdateActiveSensitivityScriptableObject(SensitivityScriptableObject sensitivityScriptableObject)
    {
        activeSensitivityScriptableObject = sensitivityScriptableObject;
    }
    
    private void WeaponSway()
    {
        Vector3 targetPosition = LissajousCurve( swayTime * activeSwayScriptableObject.swaySpeed, activeSwayScriptableObject.swayAmountA, activeSwayScriptableObject.swayAmountB) / activeSwayScriptableObject.swayScale;

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * activeSwayScriptableObject.swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }

        transform.localPosition = swayPosition;
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

        float xTilt = Mathf.Clamp(rawSineWave * activeSensitivityScriptableObject.tiltIntensity, -activeSensitivityScriptableObject.maxRotation, activeSensitivityScriptableObject.maxRotation);
        float yTilt = Mathf.Clamp(rawSineWave *activeSensitivityScriptableObject.tiltIntensity, -activeSensitivityScriptableObject.maxRotation, activeSensitivityScriptableObject.maxRotation);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(
            0f,
            0f,
            zRotation ? -xTilt : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation * targetRotation, Time.deltaTime * activeSensitivityScriptableObject.tiltSmoothness);
    }

    private void TiltWeapon()
    {
        float xInput = Input.GetAxis("Mouse X");
        float yInput = Input.GetAxis("Mouse Y");

        float xTilt = Mathf.Clamp(xInput * activeSensitivityScriptableObject.tiltIntensity, -activeSensitivityScriptableObject.maxRotation, activeSensitivityScriptableObject.maxRotation);
        float yTilt = Mathf.Clamp(yInput * activeSensitivityScriptableObject.tiltIntensity, -activeSensitivityScriptableObject.maxRotation, activeSensitivityScriptableObject.maxRotation);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(
            xRotation ? yTilt : 0f,
            yRotation ? -xTilt : 0f,
            zRotation ? -xTilt : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, originRotation * targetRotation, Time.deltaTime * activeSensitivityScriptableObject.tiltSmoothness);
    }
    
    #endregion
}
