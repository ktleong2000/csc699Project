using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_Abilities : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private S_InputReader s_inputreader;
    [SerializeField] private Animator animator;

    private bool shouldFire;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.PrimaryFireEvent += HandlePrimaryFire;
        animator = GetComponent<Animator>();
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    private void Update()
    {
        if(!IsOwner){ return; }

        if(!shouldFire){ return; }

        animator.SetBool("IsSPriFire", true);
        Debug.Log("Animating attack");
        animator.SetBool("IsSPriFire", false);
    }

    private void HandlePrimaryFire(bool shouldFire){
        this.shouldFire = shouldFire;
    }

}
