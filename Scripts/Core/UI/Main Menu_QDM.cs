using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu_QDM : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
    public async void StartHost(){
        await HostSingleton.Instance.gameManager.StartHostAsync();
    }

    public async void StartClient(){
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
}