using UnityEngine;
using System.Collections;




public class EachKey : MonoBehaviour 
{
	public Game_PinBall		_Game_PinBall;


//	// Start -----------------------------------
//	void Start () 
//	{
//	
//	}
//	
//	// Update ----------------------------------
//	void Update () 
//	{
//	
//	}


	// OnCollisionEnter -----------------------------------
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball")
		{
			GameObject goEffectTemp = Resources.Load("Prefab/ItemGet") as GameObject;
			GameObject goEffect = Instantiate(goEffectTemp) as GameObject;
			goEffect.transform.parent = gameObject.transform.parent;
			goEffect.transform.localPosition = gameObject.transform.localPosition;
			Invoke("Kill", 0.1f);
		}
	}


	void Kill()
	{
		_Game_PinBall.GetKey();
		gameObject.SetActive(false);
	}

}
