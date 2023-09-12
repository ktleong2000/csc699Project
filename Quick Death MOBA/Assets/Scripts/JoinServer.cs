using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void Join(){
        NetworkManager.Singleton.StartClient();
    }
}
