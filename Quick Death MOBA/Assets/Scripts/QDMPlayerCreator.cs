using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is a reiteration of the hero create from the given example. 
//This will spawn my playable character into the scene. And is handled on an
//independent object in the scene
public class QDMPlayerCreator : MonoBehaviour
{
	private static QDMGameManager gameManager;

	void Start()
	{
		gameManager = GameObject.Find("Game Manager").GetComponent<QDMGameManager>();
		gameManager.CreatePlayers();
	}
}