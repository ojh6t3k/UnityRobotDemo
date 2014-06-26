using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class Input_Correction : MonoBehaviour 
{
	public RobotProxy		_RobotProxy;
	public RobotConnect		_RobotConnect;
	public Input_Display	_Input_Display;
	public GameUI			_GameUI;

	public float		_Sensitivity = 1f; // 민감도(노이즈 보정)---
	public float		_Distance = 4f; // 민감도(사용 거리)---

	bool			_bIsNoiseFiltering = false;
	int				_nBufferCount = 0; // 버퍼 갯수-----
	int				_nBufferRollingCount = 0; // 내장배열번호대로 돌면서 값을 덮어 쓴다----------



	bool 	_bIsInput = false;
	
	bool	_bIsDisplay = false;
	public GameObject	_goInputPanel; // 입력상황 창------





	// 아날로그 모듈-----------------
	public ADCModule		_A_FL;
	public ADCModule		_A_F;
	public ADCModule		_A_FR;
	public ADCModule		_A_U;
	public ADCModule		_A_D;
	public ADCModule		_A_L;
	public ADCModule		_A_R;
	public ADCModule		_A_P;


	// 디지털 모듈--------------------
	public DigitalModule	_D_1;
	public DigitalModule	_D_2;
	public DigitalModule	_D_3;
	public DigitalModule	_D_4;
	public DigitalModule	_D_5;
	public DigitalModule	_D_6;
	public DigitalModule	_D_7;

	public DigitalModule	_D_L;
	public DigitalModule	_D_R;


	// 원시 데이터---------------------
	float		_fUse_A_FL_n = 0f;
	float		_fUse_A_F_n = 0f;
	float		_fUse_A_FR_n = 0f;
	float		_fUse_A_U_n = 0f;
	float		_fUse_A_D_n = 0f;
	float		_fUse_A_L_n = 0f;
	float		_fUse_A_R_n = 0f;

	
	// 가동된 데이터----------------
	public float		_fUse_A_FL = 0f;
	public float		_fUse_A_F = 0f;
	public float		_fUse_A_FR = 0f;
	public float		_fUse_A_U = 0f;
	public float		_fUse_A_D = 0f;
	public float		_fUse_A_L = 0f;
	public float		_fUse_A_R = 0f;
	public float		_fUse_A_P = 0f;

	public bool		_bUse_D_1 = false;
	public bool		_bUse_D_2 = false;
	public bool		_bUse_D_3 = false;
	public bool		_bUse_D_4 = false;
	public bool		_bUse_D_5 = false;
	public bool		_bUse_D_6 = false;
	public bool		_bUse_D_7 = false;
	public bool		_bUse_D_L = false;
	public bool		_bUse_D_R = false;

	public int		_nUse_D_L = 0;
	public int		_nUse_D_R = 0;



	// 스피드값을 얻기위한 바로 이전값---------------------
	float		_fUse_A_FL_Prev = 0f;
	float		_fUse_A_F_Prev = 0f;
	float		_fUse_A_FR_Prev = 0f;
	float		_fUse_A_U_Prev = 0f;
	float		_fUse_A_D_Prev = 0f;
	float		_fUse_A_L_Prev = 0f;
	float		_fUse_A_R_Prev = 0f;




	// 가공된 데이터의 스피드값------------------------
	public float		_fUse_A_FL_Speed = 0f;
	public float		_fUse_A_F_Speed = 0f;
	public float		_fUse_A_FR_Speed = 0f;
	public float		_fUse_A_U_Speed = 0f;
	public float		_fUse_A_D_Speed = 0f;
	public float		_fUse_A_L_Speed = 0f;
	public float		_fUse_A_R_Speed = 0f;






	
	// 캘리브레이팅용 최대 최소값---------------
	float[]		_fCal_A_FL = {512f, 512f};
	float[]		_fCal_A_F = {512f, 512f};
	float[]		_fCal_A_FR = {512f, 512f};
	float[]		_fCal_A_U = {512f, 512f};
	float[]		_fCal_A_D = {512f, 512f};
	float[]		_fCal_A_L = {512f, 512f};
	float[]		_fCal_A_R = {512f, 512f};



	// 민감도를 위한 버퍼-----------------------
	float[]		_fBuffer_FL;
	float[]		_fBuffer_F;
	float[]		_fBuffer_FR;
	float[]		_fBuffer_U;
	float[]		_fBuffer_D;
	float[]		_fBuffer_L;
	float[]		_fBuffer_R;



	// 민감도를 위한 버퍼 SPEED -----------------------
	float[]		_fBuffer_FL_Speed;
	float[]		_fBuffer_F_Speed;
	float[]		_fBuffer_FR_Speed;
	float[]		_fBuffer_U_Speed;
	float[]		_fBuffer_D_Speed;
	float[]		_fBuffer_L_Speed;
	float[]		_fBuffer_R_Speed;







	// Start --------------------------------
	void Start ()
	{
		Set_BufferCount(_Sensitivity);
	}





	// 버퍼카운트 셋팅-----------------------------------
	void Set_BufferCount(float p_Sensitivity)
	{
		_nBufferCount = (int)((1f - p_Sensitivity) * 50); // 민감도에 따른 버퍼 카운트수 결정------------
		if(_nBufferCount > 0)
		{
			_bIsNoiseFiltering = true;

			_nBufferRollingCount = 0;

			_fBuffer_FL = new float[_nBufferCount];
			_fBuffer_F = new float[_nBufferCount];
			_fBuffer_FR = new float[_nBufferCount];
			_fBuffer_U = new float[_nBufferCount];
			_fBuffer_D = new float[_nBufferCount];
			_fBuffer_L = new float[_nBufferCount];
			_fBuffer_R = new float[_nBufferCount];

			_fBuffer_FL_Speed = new float[_nBufferCount];
			_fBuffer_F_Speed = new float[_nBufferCount];
			_fBuffer_FR_Speed = new float[_nBufferCount];
			_fBuffer_U_Speed = new float[_nBufferCount];
			_fBuffer_D_Speed = new float[_nBufferCount];
			_fBuffer_L_Speed = new float[_nBufferCount];
			_fBuffer_R_Speed = new float[_nBufferCount];
		}
		else
		{
			_bIsNoiseFiltering = false;
		}

		_Sensitivity = p_Sensitivity;
		_Input_Display._dfl_Sensitivity.Text = "Sensitivity\n" + string.Format("{0:f2}", p_Sensitivity);
	}













	// 인풋을 시작 -----------------
	public void StartInput()
	{
		_bIsInput = true;
	}





	// Update -------------------------------------------------
	void Update () 
	{
		if (!_bIsInput)
			return;

		Update_ValueCorrection();
		Update_BufferRollingCount();

		Update_DisplayInput();
		Update_AnalogControl();
	}




	





	// 원시 데이터와 가공 데이터 0.0 ~ 1.0 -----------------------------------------
	void Update_ValueCorrection()
	{
		_fUse_A_FL_n = (1024f - _A_FL.Value) / 1024f; // 원시 값---------
		_fUse_A_FL = Each_ValueCorrection(_A_FL.Value , _fCal_A_FL); // 가공 값-------
		_fUse_A_FL = Each_NoiseFiltering(_fUse_A_FL, _fBuffer_FL); // 필터링 값--------
		_fUse_A_FL_Speed = Each_Speed(_fUse_A_FL, _fUse_A_FL_Prev); // 스피드 값-------
		_fUse_A_FL_Speed = Each_NoiseFiltering(_fUse_A_FL_Speed, _fBuffer_FL_Speed); // 스피드 필터링 값--------
		_fUse_A_FL_Prev = _fUse_A_FL;	// 이전값으로 쓰기 위한 값---------

		_fUse_A_F_n = (1024f - _A_F.Value) / 1024f; // 원시 값---------
		_fUse_A_F = Each_ValueCorrection(_A_F.Value , _fCal_A_F); // 가공 값-------
		_fUse_A_F = Each_NoiseFiltering(_fUse_A_F, _fBuffer_F); // 필터링 값--------
		_fUse_A_F_Speed = Each_Speed(_fUse_A_F, _fUse_A_F_Prev); // 스피드 값-------
		_fUse_A_F_Speed = Each_NoiseFiltering(_fUse_A_F_Speed, _fBuffer_F_Speed); // 스피드 필터링 값--------
		_fUse_A_F_Prev = _fUse_A_F;	// 이전값으로 쓰기 위한 값---------

		_fUse_A_FR_n = (1024f - _A_FR.Value) / 1024f; // 원시 값---------
		_fUse_A_FR = Each_ValueCorrection(_A_FR.Value , _fCal_A_FR); // 가공 값-------
		_fUse_A_FR = Each_NoiseFiltering(_fUse_A_FR, _fBuffer_FR); // 필터링 값--------
		_fUse_A_FR_Speed = Each_Speed(_fUse_A_FR, _fUse_A_FR_Prev); // 스피드 값-------
		_fUse_A_FR_Speed = Each_NoiseFiltering(_fUse_A_FR_Speed, _fBuffer_FR_Speed); // 스피드 필터링 값--------
		_fUse_A_FR_Prev = _fUse_A_FR;	// 이전값으로 쓰기 위한 값---------


		_fUse_A_U_n = (1024f - _A_U.Value) / 1024f; // 원시 값---------
		_fUse_A_U = Each_ValueCorrection(_A_U.Value , _fCal_A_U); // 가공 값-------
		_fUse_A_U = Each_NoiseFiltering(_fUse_A_U, _fBuffer_U); // 필터링 값--------
		_fUse_A_U_Speed = Each_Speed(_fUse_A_U, _fUse_A_U_Prev); // 스피드 값-------
		_fUse_A_U_Speed = Each_NoiseFiltering(_fUse_A_U_Speed, _fBuffer_U_Speed); // 스피드 필터링 값--------
		_fUse_A_U_Prev = _fUse_A_U;	// 이전값으로 쓰기 위한 값---------

		_fUse_A_D_n = (1024f - _A_D.Value) / 1024f; // 원시 값---------
		_fUse_A_D = Each_ValueCorrection(_A_D.Value , _fCal_A_D); // 가공 값-------
		_fUse_A_D = Each_NoiseFiltering(_fUse_A_D, _fBuffer_D); // 필터링 값--------
		_fUse_A_D_Speed = Each_Speed(_fUse_A_D, _fUse_A_D_Prev); // 스피드 값-------
		_fUse_A_D_Speed = Each_NoiseFiltering(_fUse_A_D_Speed, _fBuffer_D_Speed); // 스피드 필터링 값--------
		_fUse_A_D_Prev = _fUse_A_D;	// 이전값으로 쓰기 위한 값---------

		_fUse_A_L_n = (1024f - _A_L.Value) / 1024f; // 원시 값---------
		_fUse_A_L = Each_ValueCorrection(_A_L.Value , _fCal_A_L); // 가공 값-------
		_fUse_A_L = Each_NoiseFiltering(_fUse_A_L, _fBuffer_L); // 필터링 값--------
		_fUse_A_L_Speed = Each_Speed(_fUse_A_L, _fUse_A_L_Prev); // 스피드 값-------
		_fUse_A_L_Speed = Each_NoiseFiltering(_fUse_A_L_Speed, _fBuffer_L_Speed); // 스피드 필터링 값--------
		_fUse_A_L_Prev = _fUse_A_L;	// 이전값으로 쓰기 위한 값---------

		_fUse_A_R_n = (1024f - _A_R.Value) / 1024f; // 원시 값---------
		_fUse_A_R = Each_ValueCorrection(_A_R.Value , _fCal_A_R); // 가공 값-------
		_fUse_A_R = Each_NoiseFiltering(_fUse_A_R, _fBuffer_R); // 필터링 값--------
		_fUse_A_R_Speed = Each_Speed(_fUse_A_R, _fUse_A_R_Prev); // 스피드 값-------
		_fUse_A_R_Speed = Each_NoiseFiltering(_fUse_A_R_Speed, _fBuffer_R_Speed); // 스피드 필터링 값--------
		_fUse_A_R_Prev = _fUse_A_R;	// 이전값으로 쓰기 위한 값---------


		_fUse_A_P = _A_P.Value / 1024f;


		if (_D_1.Value == 1)
			_bUse_D_1 = true;
		else
			_bUse_D_1 = false;
		
		if (_D_2.Value == 1)
			_bUse_D_2 = true;
		else
			_bUse_D_2 = false;
		
		if (_D_3.Value == 1)
			_bUse_D_3 = true;
		else
			_bUse_D_3 = false;
		
		if (_D_4.Value == 1)
			_bUse_D_4 = true;
		else
			_bUse_D_4 = false;
		
		if (_D_5.Value == 1)
			_bUse_D_5 = true;
		else
			_bUse_D_5 = false;
		
		if (_D_6.Value == 1)
			_bUse_D_6 = true;
		else
			_bUse_D_6 = false;
		
		if (_D_7.Value == 1)
			_bUse_D_7 = true;
		else
			_bUse_D_7 = false;



		if (_D_L.Value == 0)
		{
			_bUse_D_L = true;
			_nUse_D_L ++;
		}
		else
		{
			_bUse_D_L = false;
			_nUse_D_L = 0;
		}


		if (_D_R.Value == 0)
		{
			_bUse_D_R = true;
			_nUse_D_R ++;
		}
		else
		{
			_bUse_D_R = false;
			_nUse_D_R = 0;
		}
	}








	// 수치보정----------------------------------
	float Each_ValueCorrection(float p_Value, float[] p_Cal)
	{
		// 실시간 캘리브레이팅 (최대최소가능값 실시간 갱신)------------
		if (p_Cal[0] > p_Value)
			p_Cal[0] = p_Value;
		else if (p_Cal[1] < p_Value)
				p_Cal[1] = p_Value;


		float ReturnValue = 0f;

		ReturnValue = (p_Cal[1] - p_Value) / (p_Cal[1] - p_Cal[0]); // 최대값 - 수치 / 실구간 량------
		ReturnValue = ReturnValue + (1f - ReturnValue) * ReturnValue * _Distance;
		if (ReturnValue >= 1f)
			ReturnValue = 1f;

		return ReturnValue;
	}




	// 버퍼 롤링카운트 갱신---------------------------
	void Update_BufferRollingCount()
	{
		_nBufferRollingCount ++;

		if (_nBufferRollingCount >= _nBufferCount)
			_nBufferRollingCount = 0;
	}



	// 노이즈 필터링-------------------------------
	float Each_NoiseFiltering(float p_Value, float[] p_Buffer)
	{
		if (!_bIsNoiseFiltering)
			return p_Value;

		float fAverage = 0f;


		p_Buffer[_nBufferRollingCount] = p_Value;

		for(int i=0; i<_nBufferCount; i++)
		{
			fAverage += p_Buffer[i];
		}

		fAverage /= (float)_nBufferCount;

		return fAverage;
	}




	// 현재 스피드 값 계산-----------------------------
	float Each_Speed(float p_Value, float p_ValuePrev)
	{
		return -(p_ValuePrev - p_Value) / Time.deltaTime * (((1f - _Sensitivity) * 48f) + 2f) ;
	}








	// 입력상황창 끄고 켜기------------------------------------
	public void OnOff_InputDisplay()
	{
		_bIsDisplay = !_bIsDisplay;
		_goInputPanel.SetActive(_bIsDisplay);
	}








	// 인풋 상황판으로 인풋정보 넘김------------------------------------------
	void Update_DisplayInput()
	{
		if (!_bIsDisplay)
			return;

		_Input_Display._fA_FL_n = _fUse_A_FL_n;
		_Input_Display._fA_FL = _fUse_A_FL;
		_Input_Display._fA_FL_Speed = _fUse_A_FL_Speed;
		_Input_Display._fCal_A_FL = _fCal_A_FL;

		_Input_Display._fA_F_n = _fUse_A_F_n;
		_Input_Display._fA_F = _fUse_A_F;
		_Input_Display._fA_F_Speed = _fUse_A_F_Speed;
		_Input_Display._fCal_A_F = _fCal_A_F;

		_Input_Display._fA_FR_n = _fUse_A_FR_n;
		_Input_Display._fA_FR = _fUse_A_FR;
		_Input_Display._fA_FR_Speed = _fUse_A_FR_Speed;
		_Input_Display._fCal_A_FR = _fCal_A_FR;


		_Input_Display._fA_U_n = _fUse_A_U_n;
		_Input_Display._fA_U = _fUse_A_U;
		_Input_Display._fA_U_Speed = _fUse_A_U_Speed;
		_Input_Display._fCal_A_U = _fCal_A_U;

		_Input_Display._fA_D_n = _fUse_A_D_n;
		_Input_Display._fA_D = _fUse_A_D;
		_Input_Display._fA_D_Speed = _fUse_A_D_Speed;
		_Input_Display._fCal_A_D = _fCal_A_D;

		_Input_Display._fA_L_n = _fUse_A_L_n;
		_Input_Display._fA_L = _fUse_A_L;
		_Input_Display._fA_L_Speed = _fUse_A_L_Speed;
		_Input_Display._fCal_A_L = _fCal_A_L;

		_Input_Display._fA_R_n = _fUse_A_R_n;
		_Input_Display._fA_R = _fUse_A_R;
		_Input_Display._fA_R_Speed = _fUse_A_R_Speed;
		_Input_Display._fCal_A_R = _fCal_A_R;


		_Input_Display._fA_P = _fUse_A_P;

		_Input_Display._bD_1 = _bUse_D_1;
		_Input_Display._bD_2 = _bUse_D_2;
		_Input_Display._bD_3 = _bUse_D_3;
		_Input_Display._bD_4 = _bUse_D_4;
		_Input_Display._bD_5 = _bUse_D_5;
		_Input_Display._bD_6 = _bUse_D_6;
		_Input_Display._bD_7 = _bUse_D_7;

		_Input_Display._bD_L = _bUse_D_L;
		_Input_Display._bD_R = _bUse_D_R;
	}







	// 가변저항으로 감도및 캘리브레이션 리셋 (입력표시창이 켜있을때만)------------------------------------
	void Update_AnalogControl()
	{
		if (_bIsDisplay)
		{
			if (_nUse_D_L == 1) // 좌버튼 누르는 순간-------------------
			{
				if (_nUse_D_R > 1) // 우버튼 누른채로 좌버튼 누르면 디스턴스 조절--------------------
				{
					_Distance ++;
					if (_Distance > 8)
						_Distance = 0;
					
					_Input_Display._dfl_Distance.Text = "Distance " + _Distance.ToString();
				}
				else // 좌버튼만 누르면 감도조절----------
				{
					Set_BufferCount(_fUse_A_P);
				}
			}

			if (_nUse_D_R == 1) // 우버튼 누르는 순간-------------------
			{
				_fCal_A_FL[0] = _fCal_A_FL[1] = 512f;
				_fCal_A_F[0] = _fCal_A_F[1] = 512f;
				_fCal_A_FR[0] = _fCal_A_FR[1] = 512f;
				_fCal_A_U[0] = _fCal_A_U[1] = 512f;
				_fCal_A_D[0] = _fCal_A_D[1] = 512f;
				_fCal_A_L[0] = _fCal_A_L[1] = 512f;
				_fCal_A_R[0] = _fCal_A_R[1] = 512f;
			}
		}
		else // ################## 게임 시작 ###########################
		{
			if (_nUse_D_L == 1) // 좌버튼 누르는 순간-------------------
			{
				_RobotConnect._goGamePrefab.BroadcastMessage("GamePlay", true);
				_GameUI.StartCounter(); // UI 초기화 및 카운트 시작-----------
			}
		}

	}




	// 게임 고유의 셋팅값---------------------------------
	public void Set_State(float p_Sensitivity, float p_Distance)
	{
		Set_BufferCount(p_Sensitivity);
		_Distance = p_Distance;
		_Input_Display._dfl_Distance.Text = "Distance " + _Distance.ToString();
	}




}
