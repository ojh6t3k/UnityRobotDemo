using UnityEngine;
using System.Collections;

public class AutoFlicker : MonoBehaviour 
{
	public UISprite		_UISprite;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		_UISprite.alpha = _UISprite.alpha - Time.deltaTime * 10;

		if (_UISprite.alpha < 0f)
			_UISprite.alpha = 1.5f;
	}
}
