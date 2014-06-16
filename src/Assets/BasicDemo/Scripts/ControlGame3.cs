using UnityEngine;
using System.Collections;
using System;
using UnityRobot;







public class ControlGame3 : MonoBehaviour 
{
	public bool _bGamePlay = false;
	public Material	_matChameleon;

	// 0 ~ 1
	public ADCModule	_adcR;
	public ADCModule	_adcG;
	public ADCModule	_adcB;





	// Start ----------------------------------------------------------
	void Start () 
	{
		_matChameleon.color = new Color (0.130f, 0.243f, 0.140f);
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_ChangeColor();
	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		_bGamePlay = p_Bool;
	}


	private void Update_ChangeColor()
	{
		_matChameleon.color = new Color (1f - _adcR.Value / 1024f, 1f - _adcG.Value / 1024f, 1f - _adcB.Value / 1024f);
	}






}





















