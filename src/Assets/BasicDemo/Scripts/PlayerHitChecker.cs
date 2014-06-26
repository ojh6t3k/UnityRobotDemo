using UnityEngine;
using System.Collections;

public class PlayerHitChecker : MonoBehaviour 
{
	public GameUI			_GameUI;

	void OnTriggerEnter (Collider p_Item)
	{
		if (p_Item.gameObject.tag == "Item")
		{
			Destroy(p_Item.gameObject);
			_GameUI.AddScore();
		}
		
	}
}
