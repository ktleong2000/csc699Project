using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QDMGameManager : MonoBehaviour
{
	public Player[] Players = new Player[2];
    private NetworkManager networkManager;
	public GameObject PlayerPrefab;
	private int currentPlayer = 1;
	private int currentPlayer2 = 2;
	private bool useNetwork;

	//Stored player objects to control on client side
	GameObject playerObj1;
	GameObject playerObj2;
	Transform playerTorso1;
	Transform playerTorso2;
	//Variables for player movement
	public float movementSpeed = 50f;
	private float horizontal = 0f;
    private float vertical = 0f;
	private float baseDamage = 100;

    void Start()
	{
		//make new script and add this
		DontDestroyOnLoad(gameObject);
		networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
		MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
		msgQueue.AddCallback(Constants.QDM_SMSG_MOVE, OnResponseMove);
		msgQueue.AddCallback(Constants.QDM_SMSG_AIM, OnResponseAim);
		msgQueue.AddCallback(Constants.QDM_SMSG_DMG, OnResponseDamage);
	}

	//Handles the movement of the game and is the method to send movement packets
	public void ProcessMove(GameObject hitObject, float x, float y)
	{
		if(Players[0].getPlayer() == null || Players[1].getPlayer() == null){
			//Save the player objects if we check that they are still null
			//at run time
			Players[0].setPlayer(playerObj1);
			Players[1].setPlayer(playerObj2);

			// Use the recursive method to find the "Ribs"
            playerTorso1 = FindDeepChild(Players[0].getPlayer().transform, "Ribs");
			playerTorso2 = FindDeepChild(Players[1].getPlayer().transform, "Ribs");
		}
		Debug.Log("ProcessMove called, useNetwork: " + useNetwork);
		//Check we are doing networking or we are doing hotseat
		//If we do networking it will hit the condition and send a
		//move request to the server via a packet.
		horizontal = x;
        vertical = y;

		if (useNetwork)
		{
			Debug.Log("Sending QDM move request with x value: " + x);
			Debug.Log("Sending QDM move request with y value: " + y);
			// Vector2 movement = new Vector2(horizontal, vertical).normalized;
        	// hitObject.transform.Translate(movement * movementSpeed * Time.deltaTime);
			networkManager.QDMSendMoveRequest(horizontal, vertical);
			Vector2 movement = new Vector2(horizontal, vertical).normalized;
        	hitObject.transform.Translate(movement * movementSpeed * Time.deltaTime);
		}else{
			//Movement logic here if we arent using networking and doing hotseating and have
			//no need for sending packets
			// Calculate movement direction and move the character
			Vector2 movement = new Vector2(horizontal, vertical).normalized;
        	hitObject.transform.Translate(movement * movementSpeed * Time.deltaTime);
		}	
	}

	//Handles the aiming for my character and will be sending aim packet data
	public void ProcessAiming(Transform torsoTransform, float angleDegrees)
	{
		if(Players[0].getPlayer() == null || Players[1].getPlayer() == null){
			//Save the player objects if we check that they are still null
			//at run time
			Players[0].setPlayer(playerObj1);
			Players[1].setPlayer(playerObj2);

			// Use the recursive method to find the "Ribs"
            playerTorso1 = FindDeepChild(Players[0].getPlayer().transform, "Ribs");
			playerTorso2 = FindDeepChild(Players[1].getPlayer().transform, "Ribs");
		}
		//Check we are doing networking or we are doing hotseat
		//If we do networking it will hit the condition and send a
		//packet.
		if (useNetwork)
		{
			// Ensure you're sending the correct data for aiming
			// This might need to be adjusted
			networkManager.QDMSendAimRequest(angleDegrees);
			// Assuming a 2D top-down setup, rotate around the X-axis
			torsoTransform.localRotation = Quaternion.Euler(angleDegrees, 0f , 0f);
		}
		else
		{
			// Assuming a 2D top-down setup, rotate around the X-axis
			torsoTransform.localRotation = Quaternion.Euler(angleDegrees, 0f , 0f);
		}   	
	}

	//This section is going to handle damaging or healing a player and will be
	//made so that it can be easily modified later
	public void ProcessHealth(GameObject hitObject, int healOrHurt)
	{
		if(Players[0].getPlayer() == null || Players[1].getPlayer() == null){
			//Save the player objects if we check that they are still null
			//at run time
			Players[0].setPlayer(playerObj1);
			Players[1].setPlayer(playerObj2);

			// Use the recursive method to find the "Ribs"
            playerTorso1 = FindDeepChild(Players[0].getPlayer().transform, "Ribs");
			playerTorso2 = FindDeepChild(Players[1].getPlayer().transform, "Ribs");
		}
		//Check we are doing networking or we are doing hotseat
		//If we do networking it will hit the condition and send a
		//packet.
		if (useNetwork)
		{
			//Send the user input, and call the corresponding heal or damage function
			//depending on the button that was pressed on other client side
			networkManager.QDMSendDamageRequest(baseDamage, healOrHurt);
			//Check the user input, and call the corresponding heal or damage function
			//depending on the button that was pressed.
			HealthSystemForDummies script = hitObject.GetComponent<HealthSystemForDummies>();
			if(healOrHurt == 0){
				//Heal hp
				script.AddToCurrentHealth(baseDamage);
			}else{
				//Subtract hp
				script.AddToCurrentHealth(baseDamage * -1);
			}
		}
		else
		{
			//Check the user input, and call the corresponding heal or damage function
			//depending on the button that was pressed.
			HealthSystemForDummies script = hitObject.GetComponent<HealthSystemForDummies>();
			if(healOrHurt == 0){
				//Heal hp
				script.AddToCurrentHealth(baseDamage);
			}else{
				//Subtract hp
				script.AddToCurrentHealth(baseDamage * -1);
			}
		}   	
	}

	//Calls the ResponseMove method for my game which moves the character after receiving
	//a response back from the network with the appropriate packet that contains data
	//necessary to move my character.
    public void OnResponseMove(ExtendedEventArgs eventArgs)
	{
		Debug.Log("Reached QDM response move");
		//Will call the Response Move Protocol for my game when we recceive the
		//response back from the network.
		QDMResponseMoveEventArgs args = eventArgs as QDMResponseMoveEventArgs;
		Debug.Log("QDM Response move user id: " + args.user_id);
		Debug.Log("QDM Response move op id: " + Constants.OP_ID);
		if (args.user_id == Constants.OP_ID)
		{
			// Get input values directly will update later
			float horizontal = args.x;
			float vertical = args.y;
			Debug.Log("QDM Response move x value: " + args.x);
			Debug.Log("QDM Response move y value: " + args.y);

			// Calculate movement direction
			Vector2 movement = new Vector2(horizontal, vertical).normalized;

			//Check if the user pressed on the right unit to move
			if(args.user_id == 1){
				Players[0].getPlayer().transform.Translate(movement * movementSpeed * Time.deltaTime);
			}else{
				Players[1].getPlayer().transform.Translate(movement * movementSpeed * Time.deltaTime);
			}
        	
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}

	public void OnResponseAim(ExtendedEventArgs eventArgs)
	{
		//Will call the Response Move Protocol for my game when we recceive the
		//response back from the network.
		QDMResponseAimEventArgs args = eventArgs as QDMResponseAimEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			
			// Get input values directly will update later
			float angleDegrees = args.angleDegrees;

			// Calculate movement direction
			// Assuming a 2D top-down setup, rotate around the X-axis
			if(args.user_id == 1){
				playerTorso1.localRotation = Quaternion.Euler(angleDegrees, 0f , 0f);
			}else{
				playerTorso2.localRotation = Quaternion.Euler(angleDegrees, 0f , 0f);
			}
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}
	public void OnResponseDamage(ExtendedEventArgs eventArgs)
	{
		//Will call the Response Move Protocol for my game when we recceive the
		//response back from the network.
		QDMResponseDamageEventArgs args = eventArgs as QDMResponseDamageEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			int healOrNot = args.healOrHurt;
			float Damage = args.Damage;
			//Check the user input, and call the corresponding heal or damage function
			//depending on the button that was pressed.
			HealthSystemForDummies script = Players[args.user_id-1].getPlayer().GetComponent<HealthSystemForDummies>();
			if(healOrNot == 0){
				//Heal hp
				script.AddToCurrentHealth(Damage);
			}else{
				//Subtract hp
				script.AddToCurrentHealth(Damage * -1);
			}
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}

	//Creates the Player Prefab for both player 1 and 2
	public void CreatePlayers()
	{
		playerObj1 = Instantiate(PlayerPrefab);
		playerObj2 = Instantiate(PlayerPrefab);
	}

	public void Init(Player player1, Player player2)
	{
		Players[0] = player1;
		Players[1] = player2;
		currentPlayer = 1;
		currentPlayer2 = 2;
		useNetwork = (!player1.IsMouseControlled || !player2.IsMouseControlled);
	}
	// Recursive method to find a transform by name
    Transform FindDeepChild(Transform parent, string name)
    {
        // Check if the current parent matches the name
        if (parent.name == name)
            return parent;

        // Recursively search each child
        foreach (Transform child in parent)
        {
            Transform result = FindDeepChild(child, name);
            if (result != null)
                return result;
        }

        // Return null if no match is found
        return null;
    }
}