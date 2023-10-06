using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Netcode;
using UnityEngine;

public class S_PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private S_InputReader s_inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 30f;

    private UnityEngine.Vector2 previousMovementInput;
    private bool isRotating = false;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // If not the owner, request ownership
            return;
        }

        animator = GetComponent<Animator>();
        s_inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        s_inputReader.MoveEvent -= HandleMove;
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        // Time.deltaTime helps with smoothing out the input and animations between
        // varying frame rates.

        // A and D controls to control the rotation, and the way the hip is facing for
        // movement.
        float xRotation = previousMovementInput.x * turningRate * Time.deltaTime;

        // Send rotation update to server and clients
        HandleRotationServerRpc(xRotation);
    }

    private void FixedUpdate()
    {
        // This will update in sync with the physics calculations of Unity rather than
        // frames to avoid desync in input and animations.

        // Code to move forward and backward based on the direction that our player is
        // facing, i.e., velocity
        if (!IsOwner)
        {
            return;
        }

        rb.velocity = (UnityEngine.Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;

        // Handle animation for the server and client
        HandleAnimationServerRpc(isRotating, rb.velocity != UnityEngine.Vector2.zero);
    }

    private void HandleMove(UnityEngine.Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    [ServerRpc]
    private void HandleRotationServerRpc(float xRotation)
    {
        // Apply rotation on the server
        bodyTransform.Rotate(xRotation, 0f, 0f);

        if (xRotation != 0)
        {
            bodyTransform.Rotate(xRotation, 0f, 0f);
            isRotating = true; // Set the flag to true while rotating
        }
        else
        {
            isRotating = false; // Set the flag to false when not rotating
        }

        // Synchronize rotation to all clients
        HandleRotationClientRpc(xRotation);
    }

    [ClientRpc]
    private void HandleRotationClientRpc(float xRotation)
    {
        // Apply the same rotation on all clients to synchronize with the server
        bodyTransform.Rotate(xRotation, 0f, 0f);

        // Set the flag based on the rotation
        isRotating = (xRotation != 0);
    }

    [ServerRpc]
    private void HandleAnimationServerRpc(bool isRotating, bool isMoving)
    {
        // Synchronize animation to all clients
        HandleAnimationClientRPC(isRotating, isMoving);
    }

    [ClientRpc]
    private void HandleAnimationClientRPC(bool isRotating, bool isMoving)
    {
        if (isRotating)
        {
            animator.enabled = false; // Disable the animator while rotating
        }
        else
        {
            animator.enabled = true; // Re-enable the animator when not rotating
        }

        animator.SetBool("IsMoving", isMoving);
    }
}
