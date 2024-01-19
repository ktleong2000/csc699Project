using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private JoinAllocation allocation;
    private const string MenuSceneName = "Main_Menu";
    public async Task<bool> InitAsync(){
        //Authenticate Player
        
        //Initialize UGS ie. Unity Game services.
        await UnityServices.InitializeAsync();

        AuthState authState = await AuthenticationWrapper.DoAuth();

        if(authState == AuthState.Authenticated){
            return true;
        }

        return false;
    }

    public void GoToMenu(){
        SceneManager.LoadScene(MenuSceneName);
    }

    public async Task StartClientAsync(string joinCodeField)
    {
        //Try to join a host allocated relay via a join code
        try{
            allocation = await Relay.Instance.JoinAllocationAsync(joinCodeField);
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
        NetworkManager.Singleton.StartClient();

        //Only the server needs to change the scene and the client can freely join the scene whenever
    }
}
