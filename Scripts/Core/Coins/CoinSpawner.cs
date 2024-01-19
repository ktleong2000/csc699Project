using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin coinPrefab;

    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private Vector2 xSPawnRange;
    [SerializeField] private Vector2 ySPawnRange;
    [SerializeField] private LayerMask layerMask;

    private Collider2D[] coinBuffer = new Collider2D[1];
    private float coinRadius;
    public override void OnNetworkSpawn()
    {
        if(!IsServer){return;}

        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;

        for(int i= 0; i< maxCoins; i++){
            SpawnCoin();
        }
    }
    private void SpawnCoin(){
        RespawningCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);

        coinInstance.SetValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();

        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin){
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector2 GetSpawnPoint(){
        float x = 0;
        float y = 0;
        while(true){
            x = Random.Range(xSPawnRange.x, xSPawnRange.y);
            y = Random.Range(ySPawnRange.x, ySPawnRange.y);
            Vector2 spawnPoint = new Vector2(x,y);
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, coinRadius, coinBuffer, layerMask);
            if(numColliders == 0){
                return spawnPoint;
            }
        }
    }
}