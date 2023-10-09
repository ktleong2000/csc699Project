using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
<<<<<<< HEAD
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
=======
    public void StartHost(){
        NetworkManager.Singleton.StartHost();
    }

>>>>>>> 20095b41a707bf4c35c76818cffeffad873c95fb
    public void StartClient(){
        NetworkManager.Singleton.StartClient();
    }
}
