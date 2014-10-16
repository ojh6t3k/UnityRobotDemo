using UnityEngine;
using System.Collections;

public class EachCollider : MonoBehaviour 
{
	public Game_PinBall		_Game_PinBall;



//	// Start ----------------------------------------------
//	void Start () 
//	{
//	
//	}
//	
//	// Update -------------------------------------------------
//	void Update () 
//	{
//	
//	}


	void OnCollisionEnter(Collision collision) 
	{
		if (collision.gameObject.tag == "Ball")
		{
			_Game_PinBall.BallBounce(Vector3.Distance(collision.rigidbody.velocity, Vector3.zero));
		}
	}









}
