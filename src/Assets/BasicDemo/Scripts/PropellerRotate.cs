using UnityEngine;
using System.Collections;

public class PropellerRotate : MonoBehaviour 
{
	public float	_Speed = 500f;


	// Start ----------------------------------------------------------
	void Start () 
	{
		transform.Rotate(0f, Random.Range(0, 360), 0f);
	}



	void Update () 
	{
		transform.Rotate(0f, Time.deltaTime * _Speed, 0f);
	}
}
