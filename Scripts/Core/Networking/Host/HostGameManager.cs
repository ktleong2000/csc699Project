using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies.Models;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private string lobbyId;
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
        //
        UnityTransport transport= NetworkManager.Singleton.GetComponent<UnityTransport>();

        //Takes the relay allocation and the connection type to get the ip and the port
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        //Once we allocate a relay create a lobby
        try{
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();

            //All lobbies are default public for now for everyone to see, but a UI can be added
            //to change this value so they won't show through search queries
            lobbyOptions.IsPrivate = false;

            //This lobby now carries the data for the relay join code. This allows for people
            //That want to access this room to be able to access this code
            lobbyOptions.Data = new Dictionary<string, DataObject>(){
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                    )
                }
            };
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(
                "My Lobby", MaxConnections, lobbyOptions);
            lobbyId = lobby.Id;

            //This will send a heartbeat request to UGS to tell them to keep this lobby active every
            //15 seconds. This lobby will otherwise automatically shutdown in case of crashes
            //so it will not cause infinite lobbies in the background
            HostSingleton.Instance.StartCoroutine(HeartbeatLobby(15));
        }catch(LobbyServiceException e){
            Debug.Log(e);
            return;
        }

        //Once relay is setup we start the hosting and then transfer to the scene

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);

        //Will ping different servers to see which one is the best to host on, and eventually
        //we will get a join code for other people to join the lobby
    }

    private IEnumerator HeartbeatLobby(float waitTimeSeconds){
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while(true){
            //Lobby makes a heartbeat request and wait for 15 seconds
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
