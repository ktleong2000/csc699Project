using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDMResponseDamageEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public float Damage { get; set; }
	public int healOrHurt { get; set; }

	public QDMResponseDamageEventArgs()
	{
		event_id = Constants.QDM_SMSG_DMG;
	}
}

public class QDMResponseDamage : NetworkResponse
{
    private int user_id;
	private float Damage;
	public int healOrHurt;
	public QDMResponseDamage()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		Damage = DataReader.ReadFloat(dataStream);
		healOrHurt = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
        QDMResponseDamageEventArgs args = new QDMResponseDamageEventArgs{
            user_id = user_id,
			Damage = Damage,
			healOrHurt = healOrHurt
        };

        return args;
    }
}