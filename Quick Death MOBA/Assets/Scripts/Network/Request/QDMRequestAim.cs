using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMRequestAim : NetworkRequest
{
	public QDMRequestAim()
	{
		request_id = Constants.QDM_CMSG_AIM;
	}

    //Create a new Packe to send over the network.
	public void send(float angleDegrees)
	{
		packet = new GamePacket(request_id);
		packet.addFloat32(angleDegrees);
	}
}