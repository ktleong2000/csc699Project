using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_Abilities : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private S_InputReader s_inputreader;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private GameObject hand;

    [Header("Settings")]
    [SerializeField] private float swingRate;


    private bool shouldFire;
    private float previousFireTime;
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
        if (!IsOwner) return;

        if (!shouldFire) return;

        if(Time.time < (1/swingRate) + previousFireTime) {return;}

        previousFireTime = Time.time;

        Debug.Log("Setting IsSPriFire to true");
        animator.SetBool("IsSPriFire", true);

        // Use a Coroutine to wait for the animation to finish.
        StartCoroutine(WaitForAnimationEnd());
    }

    private IEnumerator WaitForAnimationEnd()
    {
        Debug.Log("Waiting for animation to end...");

        yield return new WaitUntil(() =>
        {
            Debug.Log("Checking condition: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " >= 1.0f && !animator.IsInTransition(0)");
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !animator.IsInTransition(0);
        });

        Debug.Log("Animation finished. Setting IsSPriFire to false");
        animator.SetBool("IsSPriFire", false);
    }

    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
        Debug.Log("PrimaryFire event: " + shouldFire);
    }


}
