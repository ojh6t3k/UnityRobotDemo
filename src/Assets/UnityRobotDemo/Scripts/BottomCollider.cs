using UnityEngine;
using System.Collections;

public class BottomCollider : MonoBehaviour 
{
	public Game_PinBall		_Game_PinBall;


//	// Start --------------------------
//	void Start () 
//	{
//	
//	}
//	
//	// Update -------------------------
//	void Update () 
//	{
//	
//	}


	// OnCollisionEnter -----------------------------------
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball")
		{
			_Game_PinBall.GameClear();
		}
	}



}
