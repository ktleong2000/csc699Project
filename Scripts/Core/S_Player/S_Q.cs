using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_Q : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private S_InputReader s_inputreader;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float QCooldown;

    private bool shouldQ;
    private float previousQTime;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.Q_Ability += HandleQ;
        animator = GetComponent<Animator>();
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.Q_Ability -= HandleQ;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (!shouldQ) return;

        if(Time.time < (1/QCooldown) + previousQTime) {return;}

        previousQTime = Time.time;

        Debug.Log("Setting IsSQ to true");
        //animator.SetBool("IsSPriFire", true);

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

        Debug.Log("Animation finished. Setting IsSQ to false");
        //animator.SetBool("IsSPriFire", false);
    }

    private void HandleQ(bool shouldQ)
    {
        this.shouldQ = shouldQ;
        Debug.Log("Q event: " + shouldQ);
    }
}
