using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a player object trying to handle ownership
public class QDMPlayer
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public bool IsMouseControlled { get; set; }

	public QDMPlayer(int userID, string name, bool isMouseControlled)
	{
		UserID = userID;
		Name = name;
		IsMouseControlled = isMouseControlled;
	}

}