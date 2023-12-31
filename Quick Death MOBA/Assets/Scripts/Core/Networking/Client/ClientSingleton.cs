using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance;
    private ClientGameManager gameManager;

    public static ClientSingleton Instance{
        get{
            if(instance != null) { return instance; }

            instance = FindObjectOfType<ClientSingleton>();

            if(instance == null){
                Debug.LogError("No client singleton in the scene");
                return null;
            }

            return instance;
        }
    }
    void Start()
    {
        //Will persist on scene change
        DontDestroyOnLoad(gameObject);
    }
    

    public async Task CreateClient(){
        gameManager = new ClientGameManager();

        //Make sure that game manager is initialized before
        //continueing with code.
        await gameManager.InitAsync();
    }

}
