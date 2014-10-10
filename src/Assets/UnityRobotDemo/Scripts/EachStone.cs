using UnityEngine;
using System.Collections;

public class EachStone : MonoBehaviour 
{
	public Game_Space	_Game_Space;

	// Use this for initialization
	void Start () 
	{
		Invoke("Kill", 10f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!_Game_Space._bGamePlay)
			return;

		transform.Translate(0f, 0f, Time.deltaTime * 5f);
	}


	void Kill()
	{
		Destroy(gameObject);
	}
}
