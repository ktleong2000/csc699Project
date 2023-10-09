using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;
    private HostGameManager gameManager;

    public static HostSingleton Instance{
        get{
            if(instance != null) { return instance; }

            instance = FindObjectOfType<HostSingleton>();

            if(instance == null){
                Debug.LogError("No host singleton in the scene");
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
    

    public void CreateHost(){
        gameManager = new HostGameManager();

    }

}
