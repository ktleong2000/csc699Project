using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class S_E : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private S_InputReader s_inputreader;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float ECooldown;

    private bool shouldE;
    private float previousETime;
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.E_Ability += HandleQ;
        animator = GetComponent<Animator>();
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){ return; }

        s_inputreader.E_Ability -= HandleQ;
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (!shouldE) return;

        if(Time.time < (1/ECooldown) + previousETime) {return;}

        previousETime = Time.time;

        Debug.Log("Setting IsSE to true");
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

        Debug.Log("Animation finished. Setting IsSE to false");
        //animator.SetBool("IsSPriFire", false);
    }

    private void HandleQ(bool shouldE)
    {
        this.shouldE = shouldE;
        Debug.Log("E event: " + shouldE);
    }
}
