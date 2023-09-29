using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Netcode;
using UnityEngine;

public class S_PlayerMovement : NetworkBehaviour
{
    [Header ("References")]
    [SerializeField] private S_InputReader s_inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header ("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 30f;


    private UnityEngine.Vector2 previousMovementInput;
    private bool isRotating = false;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            return;
        }

        animator = GetComponent<Animator>();
        s_inputReader.MoveEvent += HandleMove;
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){
            return;
        }

        s_inputReader.MoveEvent -= HandleMove;
    }
    private void Update()
    {
        if(!IsOwner){
            return;
        }

        //Time.deltaTime helps with smoothing out the input and animations between
        //varying frame rates.

        //A and D controls to control the rotation, and the way the hip is facing for
        //movement.
        
        float xRotation = previousMovementInput.x * turningRate * Time.deltaTime;
        
        if (xRotation != 0)
        {
            bodyTransform.Rotate(xRotation, 0f, 0f);
            isRotating = true; // Set the flag to true while rotating
        }
        else
        {
            isRotating = false; // Set the flag to false when not rotating
        }
        

    }

    private void FixedUpdate() {
        //This will update in sync with the physic calculations of Unity rather than
        //frames to avoid desync in input and animations.
        
        //Code to move forward and backwards based on the direction that our player is
        //facing ie.velocity
        if(!IsOwner){return;}

        rb.velocity = (UnityEngine.Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;

        if (isRotating)
        {
            animator.enabled = false; // Disable the animator while rotating
        }
        else
        {
            animator.enabled = true; // Re-enable the animator when not rotating
        }

        if(rb.velocity != UnityEngine.Vector2.zero){
            animator.SetBool("IsMoving", true);
        }else{
            animator.SetBool("IsMoving", false);
        }
    }

    private void HandleMove(UnityEngine.Vector2 movementInput){
        previousMovementInput = movementInput;
    }
}
