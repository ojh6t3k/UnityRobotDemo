using UnityEngine;
using System.Collections;

public class EachBoxingEffect : MonoBehaviour 
{
	float		_fScale = 0f;

	public UISprite	_UISpr;


	// Start ---------------------------------------------
	void Start () 
	{
		_UISpr.enabled = false;
	}



	// Update ---------------------------------------------
	void Update () 
	{
		if (_fScale <= 0f)
			return;

		_fScale = _fScale - Time.deltaTime * 20f;


		if (_fScale >= 1f)
			gameObject.transform.localScale = Vector3.one;
		else
			gameObject.transform.localScale = Vector3.one * _fScale;

		if (_fScale <= 0f)
			_UISpr.enabled = false;

	}


	public void StartBoxingEffect()
	{
		_fScale = 3f;
		_UISpr.enabled = true;
	}



}
