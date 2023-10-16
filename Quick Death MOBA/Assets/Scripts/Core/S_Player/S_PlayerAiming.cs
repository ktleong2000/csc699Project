using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_PlayerAiming : NetworkBehaviour
{
    [SerializeField] private S_InputReader s_inputReader;
    [SerializeField] private Transform torsoTransform;
    [SerializeField] private float rotationSpeed = 5f; // Adjust this value to control rotation speed.

    //private UnityEngine.Vector2 aimPosition;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // If not the owner, request ownership
            return;
        }

        
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector2 aimScreenPosition = s_inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(aimScreenPosition.x, aimScreenPosition.y, torsoTransform.position.z));

        Vector2 directionToAim = aimWorldPosition - (Vector2)torsoTransform.position;
        float angle = Mathf.Atan2(directionToAim.y, directionToAim.x);
        float angleDegrees = angle * Mathf.Rad2Deg - 90;
        
        // Apply the rotation to the model's torso transform.
        // You may need to adjust the local axis (e.g., Vector3.right) based on your model's orientation.
        HandleAimServerRPC(angleDegrees, 0f, 0f);
    }

    // private void HandleAim(UnityEngine.Vector2 aim)
    // {
    //     aimPosition = aim;
    // }


    [ServerRpc]
    private void HandleAimServerRPC(float angleDegreesX, float angleDegreesY, float angleDegreesZ)
    {
        // Call the client RPC to update the client's view.
        HandleAimClientRPC(angleDegreesX, angleDegreesY, angleDegreesZ);
    }

    [ClientRpc]
    private void HandleAimClientRPC(float angleDegreesX, float angleDegreesY, float angleDegreesZ)
    {
        // Update the client's view with the synchronized rotation.
        torsoTransform.localRotation = Quaternion.Euler(angleDegreesX, angleDegreesY, angleDegreesZ);
    }
}






















