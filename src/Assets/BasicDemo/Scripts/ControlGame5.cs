using UnityEngine;
using System.Collections;
using System;
using UnityRobot;





public class ControlGame5 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	public Input_Correction		_input_Correction;
	public GameUI			_GameUI;

	public GameObject	_goMonster;
	public Animation		_ani;

	bool	_bIsHit = false;

	Vector3	_v3LR_Position = Vector3.zero;
	Vector3	_v3LR_Rotation = Vector3.zero;
	float	_fWeight = 0f;

	float	_fSine = 1f;




	// Start ----------------------------------------------------------
	void Start () 
	{
		Reset();
		_input_Correction.Set_State(0.5f, 2f); // 게임 고유의 셋팅 값---------
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Sencor();
		Update_AutoPosition();
	}





	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;

		_bGamePlay = p_Bool;
	}






	// Update_Sencor ----------------------------------------------------
	void Update_Sencor()
	{
		if (_bIsHit)
			return;

		if (_input_Correction._fUse_A_F_Speed > 20f)
		{
			_bIsHit = true;
			_ani.Stop("wound");
			_ani.Play("wound");
			_GameUI.AddScore((int)_input_Correction._fUse_A_F_Speed);
			Invoke("Reset" , 0.5f);
		}
		else if (_input_Correction._fUse_A_FL_Speed > 20f)
		{
			_bIsHit = true;
			_ani.Stop("wound");
			_ani.Play("wound");
			_fWeight = 1f;
			_fSine = 1f;
			_GameUI.AddScore((int)_input_Correction._fUse_A_FL_Speed);
			Invoke("Reset" , 0.5f);
		}
		else if (_input_Correction._fUse_A_FR_Speed > 20f)
		{
			_bIsHit = true;
			_ani.Stop("wound");
			_ani.Play("wound");
			_fWeight = 1f;
			_fSine = -1f;
			_GameUI.AddScore((int)_input_Correction._fUse_A_FR_Speed);
			Invoke("Reset" , 0.5f);
		}

	}



	//-----------------------------------------------
	void Update_AutoPosition()
	{
		if (_fWeight <= 0f)
			return;

		_goMonster.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0.3f, 0f, 0f) * _fSine, _fWeight);
		_goMonster.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, 60f) * _fSine, _fWeight);

		_fWeight = _fWeight - Time.deltaTime * 2f ;

		if (_fWeight < 0f)
			_fWeight = 0f;
	}





	//------------------------------------
	void Reset()
	{
		_bIsHit = false;
		_ani.CrossFade("idle", 0.3f);
	}













}





















