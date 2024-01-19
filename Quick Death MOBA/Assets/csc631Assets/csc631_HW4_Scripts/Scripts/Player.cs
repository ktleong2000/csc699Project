using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public Color Color { get; set; }
	public Hero[] Heroes { get; set; }
	public bool IsMouseControlled { get; set; }

	private int heroCount = 0;

	//QDM assets to be stored for networking purposes
	GameObject playerPrefab;

	public Player(int userID, string name, Color color, bool isMouseControlled)
	{
		UserID = userID;
		Name = name;
		Color = color;
		Heroes = new Hero[5];
		IsMouseControlled = isMouseControlled;
	}
	public Player(int userID, string name, bool isMouseControlled)
	{
		UserID = userID;
		Name = name;
		IsMouseControlled = isMouseControlled;
	}

	public void AddHero(Hero hero)
	{
		Heroes[heroCount++] = hero;
		hero.Owner = this;
	}
	//QDM get and set code
	public void setPlayer(GameObject Prefab)
	{
		this.playerPrefab = Prefab;
	}
	public GameObject getPlayer()
	{
		return playerPrefab;
	}
}
