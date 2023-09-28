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

    [Header ("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 30f;

    private UnityEngine.Vector2 previousMovementInput;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            return;
        }

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
        bodyTransform.Rotate(xRotation, 0f, 0f);
    }

    private void FixedUpdate() {
        //This will update in sync with the physic calculations of Unity rather than
        //frames to avoid desync in input and animations.
        
        //Code to move forward and backwards based on the direction that our player is
        //facing ie.velocity
        if(!IsOwner){return;}

        rb.velocity = (UnityEngine.Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
    }

    private void HandleMove(UnityEngine.Vector2 movementInput){
        previousMovementInput = movementInput;
    }
}
