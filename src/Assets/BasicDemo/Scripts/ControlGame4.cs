using UnityEngine;
using System.Collections;
using System;
using UnityRobot;





public class ControlGame4 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	
	public GameObject		_goCamera;
	public GameObject		_goCameraCase;


	public Input_Correction		_input_Correction;


	float	_fCamAng = 0f;
	float	_fForWard = 0f;


	float	_fSign = 1f;


	// Start ----------------------------------------------------------
	void Start () 
	{
		_input_Correction.Set_State(0.1f, 6f); // 게임 고유의 셋팅 값---------
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Camera();
	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;
		
		_bGamePlay = p_Bool;
	}






	// Update_Camera ----------------------------------------------------
	void Update_Camera()
	{
		_fCamAng = -(_input_Correction._fUse_A_R - _input_Correction._fUse_A_L) * 30f;
		_fForWard = _input_Correction._fUse_A_U * 20f;

		if (_fCamAng > 0)
			_fSign = 1f;
		else
			_fSign = -1f;




		_goCameraCase.transform.localEulerAngles = new Vector3(0f, _fCamAng + (_fSign * (_fForWard * Mathf.Abs(_fCamAng / 30f))), 0f);


		_goCamera.transform.localPosition = new Vector3(0f, 0f, _fForWard / 2.5f);

	}







}





















