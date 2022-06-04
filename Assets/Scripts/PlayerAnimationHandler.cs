using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private GameObject nonADSCrosshair;
    [SerializeField] private GameObject adsCrosshair;
    [SerializeField] private float corsshairSwitchTime;

    private void Start()
    {
        PlayerMovement.PlayerStateChanged += HandleStateChanged;
    }

    private void OnDestroy() 
    {
        PlayerMovement.PlayerStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged()
    {
        switch (PlayerMovement.GetPlayerState)
        {
            case PlayerState.Idle:
                playerAnimator.SetBool("PlayerRunning", false);
                weaponAnimator.SetBool("M4Running", false);
                playerAnimator.SetBool("PlayerAiming", false);
                weaponAnimator.SetBool("M4Aiming", false);
                cameraAnimator.SetBool("CameraAiming", false);
                StartCoroutine(UpdateCrosshair(false));
                break;
            case PlayerState.Walking:
                playerAnimator.SetBool("PlayerRunning", false);
                weaponAnimator.SetBool("M4Running", false);
                playerAnimator.SetBool("PlayerAiming", false);
                weaponAnimator.SetBool("M4Aiming", false);
                cameraAnimator.SetBool("CameraAiming", false);
                StartCoroutine(UpdateCrosshair(false));
                break;
            case PlayerState.Jumping:
                playerAnimator.SetBool("PlayerRunning", false);
                weaponAnimator.SetBool("M4Running", false);
                playerAnimator.SetBool("PlayerAiming", false);
                weaponAnimator.SetBool("M4Aiming", false);
                cameraAnimator.SetBool("CameraAiming", false);
                StartCoroutine(UpdateCrosshair(false));
                break;
            case PlayerState.Running:
                playerAnimator.SetBool("PlayerRunning", true);
                weaponAnimator.SetBool("M4Running", true);
                playerAnimator.SetBool("PlayerAiming", false);
                weaponAnimator.SetBool("M4Aiming", false);
                cameraAnimator.SetBool("CameraAiming", false);
                StartCoroutine(UpdateCrosshair(false));
                break;  
            case PlayerState.Aiming:
                playerAnimator.SetBool("PlayerAiming", true);
                weaponAnimator.SetBool("M4Aiming", true);
                cameraAnimator.SetBool("CameraAiming" ,true);
                StartCoroutine(UpdateCrosshair(true));
                break;
        }
    }

    private IEnumerator UpdateCrosshair(bool ads)
    {
        yield return new WaitForSeconds(corsshairSwitchTime);
        if(ads)
        {
            adsCrosshair.SetActive(true);
            nonADSCrosshair.SetActive(false);
        }
        else
        {
            adsCrosshair.SetActive(false);
            nonADSCrosshair.SetActive(true);
        }
    }
}
