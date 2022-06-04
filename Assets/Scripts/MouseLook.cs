using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float nonShootingSensitivity = 100f;
    [SerializeField] private float shootingSensitivity = 50f;
    
    [SerializeField] private GameObject gunCamera;

    private float mouseSensitivity = 100f;
    
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;    
    }

    private void Update() 
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;  
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
        gunCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void UpdateSensToShootingSens()
    {
        if(mouseSensitivity != shootingSensitivity && PlayerMovement.GetPlayerState == PlayerState.Aiming)
        {
            mouseSensitivity = shootingSensitivity;
        }
    }

    public void UpdateSensToNonShootingSens()
    {
        if(mouseSensitivity != nonShootingSensitivity)
        {
            mouseSensitivity = nonShootingSensitivity;
        }
    }
}
 