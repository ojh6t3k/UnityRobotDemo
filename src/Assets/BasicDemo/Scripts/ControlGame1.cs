using UnityEngine;
using System.Collections;
using System;
using UnityRobot;








public class ControlGame1 : MonoBehaviour 
{
	public bool _bGamePlay = false;


	public GameObject		_goMazeSet;
	public GameObject		_goMaze;

	public GameObject		_goBall;

	public GameObject		_goPumpkinSetTemp;
	public GameObject		_goPumpkinSet;

	bool	_bIsActiveMaze = true;

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
		Update_Controll_Maze();
		Update_BallReset();
		Update_Check_Button();
	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;

		if (p_Bool)
		{
			_bGamePlay = p_Bool;
			//_goBall.SetActive(p_Bool);
			Reset_Game();
		}
		else
		{
			_bGamePlay = p_Bool;
			_goBall.SetActive(p_Bool);
		}
	}





	// 밸런스 출력----------------------------------------------------
	void Update_Output_Ballence()
	{
		_fAngleX = _input_Correction._fUse_A_U - _input_Correction._fUse_A_D;
		_fAngleZ = _input_Correction._fUse_A_L - _input_Correction._fUse_A_R;
		_fHight = (_input_Correction._fUse_A_L + _input_Correction._fUse_A_R + _input_Correction._fUse_A_U + _input_Correction._fUse_A_D) * 0.25f;
	}








	void Update_Controll_Maze()
	{
		_goMazeSet.transform.localEulerAngles = new Vector3(_fAngleX, 0f, _fAngleZ) * 90f;
		_goMazeSet.transform.localPosition = new Vector3(0f, _fHight * -1f, 0f);
	}



	void Update_BallReset()
	{
		if(_goBall.transform.localPosition.y < -3f)
			_goBall.transform.localPosition = new Vector3(0f, 1.6f, 0f);
	}




	// 우버튼 체크------------------------
	void Update_Check_Button()
	{
		if (_input_Correction._nUse_D_R == 1)
		{
			_bIsActiveMaze = !_bIsActiveMaze;
			_goMaze.SetActive(_bIsActiveMaze);
		}
	}





	// 게임 리셋-----------------------------------
	void Reset_Game()
	{
		_goMazeSet.transform.localEulerAngles = Vector3.zero;
		_goMazeSet.transform.localPosition = Vector3.zero;

		_goMaze.SetActive(true);

		Destroy(_goPumpkinSet);

		_goPumpkinSet = Instantiate(_goPumpkinSetTemp) as GameObject;
		_goPumpkinSet.transform.parent = _goMazeSet.transform;
		_goPumpkinSet.transform.localPosition = new Vector3(0.125f, 0f, -0.125f);
		_goPumpkinSet.transform.localEulerAngles = Vector3.zero;


		_goBall.SetActive(true);
		_goBall.transform.localPosition = new Vector3(0f, 1.6f, 0f);

	}









}





















