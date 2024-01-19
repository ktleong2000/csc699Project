using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMResponseMoveEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public float x { get; set; } // The x coordinate of the target location
	public float y { get; set; } // The y coordinate of the target location

	public QDMResponseMoveEventArgs()
	{
		event_id = Constants.QDM_SMSG_MOVE;
	}
}

public class QDMResponseMove : NetworkResponse
{
    private int user_id;
	private float x;
	private float y;
	public QDMResponseMove()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		x = DataReader.ReadFloat(dataStream);
		y = DataReader.ReadFloat(dataStream);
	}

	public override ExtendedEventArgs process()
	{
        QDMResponseMoveEventArgs args = new QDMResponseMoveEventArgs{
            user_id = user_id,
			x = x,
			y = y
        };

        return args;
    }
}
