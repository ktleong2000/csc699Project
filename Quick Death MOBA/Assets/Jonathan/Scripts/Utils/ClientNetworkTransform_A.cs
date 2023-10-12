using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    //At all times, owner can commit to transform
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    //As long as client is connected and owner, send msg to server to try and transform the object in real time
    protected override void Update()
    {
        CanCommitToTransform = IsOwner;
        base.Update();
        if(NetworkManager != null)
        {
            if(NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if(CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }

    }
    //Sets server to unauthoritive, allows client to transform
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
