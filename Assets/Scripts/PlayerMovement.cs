using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float runningSpeedMultiplier = 1.81f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = .4f;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float walkingStateThreshold;
    [SerializeField] private static PlayerState playerState = PlayerState.Idle;

    public static PlayerState GetPlayerState { get { return playerState; } }

    public static event Action PlayerStateChanged;

    private Vector3 velocity;
    private Vector3 movementVector;
    float verticalMovement;
    float horizontalMOvement;

    bool isGrounded;

    private void Update() 
    {   
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        horizontalMOvement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        movementVector = transform.right * horizontalMOvement + transform.forward * verticalMovement;

        if(Input.GetButton("Fire3") && isGrounded && playerState != PlayerState.Aiming)
        {
            if(Vector3.Dot(transform.forward, movementVector.normalized) > .68f)
            {
                Debug.Log(Vector3.Dot(transform.forward, movementVector.normalized));
                movementVector *= runningSpeedMultiplier;
            }
        }

        controller.Move(movementVector * Time.deltaTime * speed);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

     
        UpdatePlayerState();
        

    }

    private void UpdatePlayerState()
    {
        PlayerState prevPlayerState = playerState;

        if (!Input.GetButton("Fire2"))
        {
            if (!isGrounded)
            {
                playerState = PlayerState.Jumping;
            }
            else if (Mathf.Abs(horizontalMOvement) < walkingStateThreshold && Mathf.Abs(verticalMovement) < walkingStateThreshold)
            {
                playerState = PlayerState.Idle;
            }
            else if (Input.GetButton("Fire3") && isGrounded && Vector3.Dot(transform.forward, movementVector.normalized) > .68f)
            {
                playerState = PlayerState.Running;
            }
            else
            {
                playerState = PlayerState.Walking;
            }
        }
        else
        {
            playerState = PlayerState.Aiming;
        }

        
        if(prevPlayerState != playerState)
        {
            PlayerStateChanged?.Invoke();
        }
    }
}
