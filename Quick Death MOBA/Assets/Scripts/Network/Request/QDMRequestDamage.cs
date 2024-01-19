using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMRequestDamage : NetworkRequest
{
	public QDMRequestDamage()
	{
		request_id = Constants.QDM_CMSG_DMG;
	}

    //Create a new Packe to send over the network.
	public void send(float dmg,int healOrHurt)
	{
		packet = new GamePacket(request_id);
		packet.addFloat32(dmg);
		packet.addInt32(healOrHurt);
	}
}