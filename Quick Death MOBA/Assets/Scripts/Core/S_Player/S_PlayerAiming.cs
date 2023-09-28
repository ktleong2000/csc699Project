using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_PlayerAiming : NetworkBehaviour
{
    [SerializeField] private S_InputReader s_inputReader;
    [SerializeField] private Transform torsoTransform;
    [SerializeField] private float rotationSpeed = 5f; // Adjust this value to control rotation speed.

    private Vector2 lastAimPosition;

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector2 aimScreenPosition = s_inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(aimScreenPosition.x, aimScreenPosition.y, torsoTransform.position.z));

        // Calculate the direction from the torso's current position to the aim position.
        Vector2 directionToAim = aimWorldPosition - (Vector2)torsoTransform.position;

        // Calculate the angle in radians between the direction and the local forward direction (adjust this based on your model).
        float angle = Mathf.Atan2(directionToAim.y, directionToAim.x);

        // Convert angle to degrees and apply rotation to the torso around its local X-axis (left and right).
        float angleDegrees = angle * Mathf.Rad2Deg - 90;
        
        // Apply the rotation to the model's torso transform.
        // You may need to adjust the local axis (e.g., Vector3.right) based on your model's orientation.
        torsoTransform.localRotation = Quaternion.Euler(angleDegrees, 0f, 0f);
    }
}






















