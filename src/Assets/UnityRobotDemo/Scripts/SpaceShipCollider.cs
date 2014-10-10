using UnityEngine;
using System.Collections;

public class SpaceShipCollider : MonoBehaviour 
{
	public Game_Space		_game_Space;



//	// Start --------------------------------------------------------
//	void Start () 
//	{
//	
//	}
//	
//	// Update -------------------------------------------------------
//	void Update () 
//	{
//	
//	}



	// OnCollisionEnter -----------------------------------
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Item")
		{
			GameObject goEffectTemp = Resources.Load("Prefab/ItemGet_Fuel") as GameObject;
			GameObject goEffect = Instantiate(goEffectTemp) as GameObject;
			goEffect.transform.position = other.gameObject.transform.position;
			_game_Space.RechargeFuel();

			Destroy(other.gameObject);
		}

		if (other.gameObject.tag == "Stone")
		{
			_game_Space.Crash();
		}


	}

}
