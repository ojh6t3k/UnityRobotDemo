using UnityEngine;
using System.Collections;
using System;
using UnityRobot;





public class ControlGame2 : MonoBehaviour 
{
	public bool _bGamePlay = false;
	
	public GameObject		_goGameRoot;
	public GameObject		_goCamera;
	public GameObject		_goFighter;

	public GameObject		_goItemSetTemp;
	public GameObject		_goItemSet;


	public Input_Correction		_input_Correction;
	
	// 밸런스 값---------------
	float	_fAngleX = 0f;
	float	_fAngleZ = 0f;
	float	_fHight = 0f;
	//------------------------





	// Start ----------------------------------------------------------
	void Start () 
	{
		_input_Correction.Set_State(0.8f, 0f); // 게임 고유의 셋팅 값---------
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Output_Ballence();
		Update_Controll_Fighter();

	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;

		if (p_Bool)
		{
			_bGamePlay = p_Bool;
			//_goFighter.SetActive(p_Bool);
			Reset_Game();
		}
		else
		{
			_bGamePlay = p_Bool;
			_goFighter.SetActive(p_Bool);
		}
	}



	// 밸런스 출력----------------------------------------------------
	void Update_Output_Ballence()
	{
		_fAngleX = _input_Correction._fUse_A_U - _input_Correction._fUse_A_D;
		_fAngleZ = _input_Correction._fUse_A_L - _input_Correction._fUse_A_R;
	}




	void Update_Controll_Fighter()
	{
		if (!_goFighter.activeSelf)
			return;

		_goFighter.transform.localEulerAngles = new Vector3(_fAngleX, 0f, _fAngleZ) * 60f;


	
		_goCamera.transform.Translate(0f, Time.deltaTime * _fAngleX * -2f, Time.deltaTime * 2f);
		_goCamera.transform.Rotate(0f, Time.deltaTime * -_fAngleZ * 60f, 0f);
		_goCamera.transform.localEulerAngles = new Vector3(_goCamera.transform.localEulerAngles.x, _goCamera.transform.localEulerAngles.y, 0f);

		if (_goCamera.transform.localPosition.y < 0f)
			_goCamera.transform.localPosition = new Vector3(_goCamera.transform.localPosition.x, 0f, _goCamera.transform.localPosition.z);

	}






	// 게임 리셋-----------------------------------
	void Reset_Game()
	{
		_goCamera.transform.localEulerAngles = Vector3.zero;
		_goCamera.transform.localPosition = new Vector3(0f, 0f, -5f);


		Destroy(_goItemSet);
		
		_goItemSet = Instantiate(_goItemSetTemp) as GameObject;
		_goItemSet.transform.parent = _goGameRoot.transform;
		_goItemSet.transform.localPosition = Vector3.zero;



		_goFighter.SetActive(true);
		_goFighter.transform.localEulerAngles = Vector3.zero;
		_goFighter.transform.localPosition = new Vector3(0f, -0.1354286f, 0.3271622f);
	}




}





















