using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS.Controls
{
    public class FPSCharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        [SerializeField] private float movementSpeed;

        private Vector2 movement;

        private Controls controls;

        private void Awake() {
            controls = new Controls();
        }

        private void OnEnable() 
        {
            controls.Enable();
        }

        private void OnDisable() 
        {
            controls.Disable();
        }

        private void Start() {
            controls.Player.Movement.performed += ctx => SetMovement( ctx.ReadValue<Vector2>());
            controls.Player.Movement.canceled += ctx => ResetMovement();
        }

        private void SetMovement(Vector2 movementVector)
        {
            movement = movementVector;
        }

        private void ResetMovement()
        {
            movement = Vector2.zero;
        }

        private void Update() 
        {
            HandleMovement();    
        }

        private void HandleMovement()
        {
            float forward = movement.x * Time.deltaTime * movementSpeed;
            float right = -movement.y * Time.deltaTime * movementSpeed;

            characterController.Move(transform.forward * forward + transform.right * right);
        }
    }

}

