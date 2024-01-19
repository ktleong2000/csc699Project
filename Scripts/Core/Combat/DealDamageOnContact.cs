using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    //A ulong is an integer that can be very big, and can only be positive
    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId){
        this.ownerClientId = ownerClientId;
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        //Check if we set the collider
        if(collider == null){return;}

        if(collider.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj)){
            if(netObj.OwnerClientId == netObj.OwnerClientId){
                return;
            }
        }

        //Try to get the attached health component
        if(collider.attachedRigidbody.TryGetComponent<Health>(out Health health)){
            health.TakeDamage(damage);
        }
    }
}
