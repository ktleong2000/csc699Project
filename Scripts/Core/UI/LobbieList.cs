using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbieList : MonoBehaviour
{
    [SerializeField] private Transform lobbyItemParent;
    [SerializeField] private LobbyItem lobbyItemPrefab;
    private bool isJoining;
    private bool isRefreshing;
    private void OnEnable() {
        RefreshList();
    }
    public async void RefreshList(){
        if(isRefreshing){return;}

        isRefreshing = true;

        try{
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            //This can be used to filter out lobbies
            options.Filters = new List<QueryFilter>(){
                //This basically makes it so that it only shows lobbies with slots
                //available that is greater than 0
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"
                ),
                //This checks whether the room has been locked by the user
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"
                )
            };

            //Apply the query
            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            //Destroy lobby buttons
            foreach(Transform child in lobbyItemParent){
                Destroy(child.gameObject);
            }

            //Spawn in the new lobby buttons
            foreach(Lobby lobby in lobbies.Results){
                LobbyItem lobbyItem = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyItem.Initialise(this, lobby);
            }
        }catch(LobbyServiceException e){
            Debug.Log(e);
        }


        isRefreshing = false;
    }
    public async void JoinAsync(Lobby lobby){
        if(isJoining){return;}
        isJoining = true;
        try{
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value;

            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        }catch(LobbyServiceException e){
            Debug.Log(e);
        }

        isJoining = false;    }
}
