using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    //A value that the client can handle, but only be seen by server
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D collider){
        if(!collider.TryGetComponent<Coin>(out Coin coin)) {return;}

        int coinValue = coin.Collect();

        if(!IsServer) {return;}

        TotalCoins.Value += coinValue;
    }
}
