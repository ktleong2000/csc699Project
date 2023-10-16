using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private const int MaxConnections = 20;
    private const string GameSceneName = "QMOBA_BV1";
    public async Task StartHostAsync(){
        //Gets Relay to create an allocation for the host with specified max connections
        try{
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }catch(Exception e){
            Debug.Log(e);
            return;
        }

        try{
            //Uses the allocation id from relay allocation to get a join code for people to join the room.
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }catch(Exception e){
            Debug.Log(e);
            return;
        }
        
        //Get us a reference to the network manager game object and get the transport component
        UnityTransport transport= NetworkManager.Singleton.GetComponent<UnityTransport>();

        //Takes the relay allocation and the connection type to get the ip and the port
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        //Once relay is setup we start the hosting and then transfer to the scene
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);

        //Will ping different servers to see which one is the best to host on, and eventually
        //we will get a join code for other people to join the lobby
    }
}
