using UnityEngine;
using System.Collections;

public class PropellerRotate : MonoBehaviour 
{


	void Update () 
	{
		transform.Rotate(0f, Time.deltaTime * 500f, 0f);
	}
}
