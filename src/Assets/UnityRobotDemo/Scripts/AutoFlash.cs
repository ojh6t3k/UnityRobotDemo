using UnityEngine;
using System.Collections;

public class AutoFlash : MonoBehaviour 
{
	public UISprite	_UISpr;


	// Use this for initialization
	void Start () 
	{
		_UISpr.enabled = false;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void StartFlash(float p_Time)
	{
		_UISpr.enabled = true;
		Invoke("EndFlash", p_Time);
	}

	public void EndFlash()
	{
		_UISpr.enabled = false;
	}


}
