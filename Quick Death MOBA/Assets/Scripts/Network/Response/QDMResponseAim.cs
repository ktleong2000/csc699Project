using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMResponseAimEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public float angleDegrees { get; set; }
	

	public QDMResponseAimEventArgs()
	{
		event_id = Constants.QDM_SMSG_AIM;
	}
}

public class QDMResponseAim : NetworkResponse
{
    private int user_id;
	private float angleDegrees;
	public QDMResponseAim()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		angleDegrees = DataReader.ReadFloat(dataStream);
	}

	public override ExtendedEventArgs process()
	{
        QDMResponseAimEventArgs args = new QDMResponseAimEventArgs{
            user_id = user_id,
			angleDegrees = angleDegrees
        };

        return args;
    }
}