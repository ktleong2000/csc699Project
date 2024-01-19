using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMRequestMove : NetworkRequest
{
	public QDMRequestMove()
	{
		request_id = Constants.QDM_CMSG_MOVE;
	}

    //Create a new Packe to send over the network.
	public void send(float x, float y)
	{
		packet = new GamePacket(request_id);
		packet.addFloat32(x);
		packet.addFloat32(y);
	}
}