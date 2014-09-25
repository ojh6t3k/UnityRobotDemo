using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityRobot;


public class Input_Correction : MonoBehaviour 
{
	public RobotProxy		_RobotProxy;
	public RobotConnect		_RobotConnect;
	public EachGauge[]		_EachGaugeA = new EachGauge[8];
	public EachGauge[]		_EachGaugeD = new EachGauge[16];

	public bool[]		_bShowAnalogue = new bool[8];
	public bool[]		_bShowDigital = new bool[16];
	public string[]		_strAName = new string[8];
	public string[]		_strDName = new string[16];
	public UISprite[]	_UIsprAnalogue = new UISprite[8];
	public UISprite[]	_UIsprDigital = new UISprite[16];


	public UILabel		_UILblSensitivity;
	public UILabel		_UILblDistance;

	public int			_Sensitivity = 5; // 민감도(노이즈 보정 1-10)---
	public float		_Distance = 4f; // 민감도(사용 거리 0-7)---

	bool			_bIsNoiseFiltering = false;
	int				_nBufferCount = 0; // 버퍼 갯수-----
	int				_nBufferRollingCount = 0; // 내장배열번호대로 돌면서 값을 덮어 쓴다----------








	// 아날로그 모듈-----------------
	public ADCModule[]		_A = new ADCModule[8];

	// 디지털 모듈--------------------
	public DigitalModule[]	_D = new DigitalModule[16];



	// 아날로그 원시(실제입력) 데이터---------------------
	float[]		_fUse_n_A = new float[8];

	// 가공된 데이터----------------
	public float[]		_fUse_A = new float[8];
	public int[]		_nUse_D = new int[16]; // Off:0,  On:1~증가  1(누른 순간 값)

	// 스피드값을 얻기위한 바로 이전값---------------------
	float[]		_fUse_Prev_A = new float[8];

	// 가공된 데이터의 스피드값------------------------
	public float[]		_fUse_Speed_A = new float[8];






	
	// 캘리브레이팅용 최대 최소값---------------
	float[]		_fCal_A0 = {512f, 512f};
	float[]		_fCal_A1 = {512f, 512f};
	float[]		_fCal_A2 = {512f, 512f};
	float[]		_fCal_A3 = {512f, 512f};
	float[]		_fCal_A4 = {512f, 512f};
	float[]		_fCal_A5 = {512f, 512f};
	float[]		_fCal_A6 = {512f, 512f};
	float[]		_fCal_A7 = {512f, 512f};
	List<float[]>	_lstCal_A = new List<float[]>();


	// 민감도를 위한 버퍼-----------------------
	float[]		_fBuffer_A0;
	float[]		_fBuffer_A1;
	float[]		_fBuffer_A2;
	float[]		_fBuffer_A3;
	float[]		_fBuffer_A4;
	float[]		_fBuffer_A5;
	float[]		_fBuffer_A6;
	float[]		_fBuffer_A7;
	List<float[]> _lstBuffer_A = new List<float[]>();




	// 민감도를 위한 버퍼 SPEED -----------------------
	float[]		_fBuffer_Speed_A0;
	float[]		_fBuffer_Speed_A1;
	float[]		_fBuffer_Speed_A2;
	float[]		_fBuffer_Speed_A3;
	float[]		_fBuffer_Speed_A4;
	float[]		_fBuffer_Speed_A5;
	float[]		_fBuffer_Speed_A6;
	float[]		_fBuffer_Speed_A7;
	List<float[]> _lstBuffer_Speed_A = new List<float[]>();





	// Start --------------------------------
	void Start ()
	{
		_lstCal_A.Add (_fCal_A0);
		_lstCal_A.Add (_fCal_A1);
		_lstCal_A.Add (_fCal_A2);
		_lstCal_A.Add (_fCal_A3);
		_lstCal_A.Add (_fCal_A4);
		_lstCal_A.Add (_fCal_A5);
		_lstCal_A.Add (_fCal_A6);
		_lstCal_A.Add (_fCal_A7);

		Set_BufferCount(_Sensitivity);
		_UILblDistance.text = _Distance.ToString();


		for (int i=0; i < 8; i++) // 아날로그 8개----
		{
			if (_bShowAnalogue[i])
			{
				if (_strAName[i] != "")
					_EachGaugeA[i]._UIlbl_Name.text = _strAName[i];
			}
			else
				_UIsprAnalogue[i].alpha = 0.1f;
		}
		for (int i=0; i < 16; i++) // 디지털 16개----
		{
			if (_bShowDigital[i])
			{
				if (_strDName[i] != "")
					_EachGaugeD[i]._UIlbl_Name.text = _strDName[i];
			}
			else
				_UIsprDigital[i].alpha = 0.1f;
		}
	}









	// 감도 조절 ------------------------------------
	public void SetSensitivityUp()
	{
		if (_Sensitivity < 10)
			Set_BufferCount(++_Sensitivity);
	}

	public void SetSensitivityDown()
	{
		if (_Sensitivity > 1)
			Set_BufferCount(--_Sensitivity);
	}



	// 버퍼카운트 셋팅-----------------------------------
	void Set_BufferCount(int p_Sensitivity)
	{
		_nBufferCount = (10 - p_Sensitivity) * 5; // 민감도에 따른 버퍼 카운트수 결정------------
		if(_nBufferCount > 0)
		{
			_bIsNoiseFiltering = true;

			_nBufferRollingCount = 0;


			_fBuffer_A0 = new float[_nBufferCount];
			_fBuffer_A1 = new float[_nBufferCount];
			_fBuffer_A2 = new float[_nBufferCount];
			_fBuffer_A3 = new float[_nBufferCount];
			_fBuffer_A4 = new float[_nBufferCount];
			_fBuffer_A5 = new float[_nBufferCount];
			_fBuffer_A6 = new float[_nBufferCount];
			_fBuffer_A7 = new float[_nBufferCount];

			_lstBuffer_A.Clear();
			_lstBuffer_A.Add (_fBuffer_A0);
			_lstBuffer_A.Add (_fBuffer_A1);
			_lstBuffer_A.Add (_fBuffer_A2);
			_lstBuffer_A.Add (_fBuffer_A3);
			_lstBuffer_A.Add (_fBuffer_A4);
			_lstBuffer_A.Add (_fBuffer_A5);
			_lstBuffer_A.Add (_fBuffer_A6);
			_lstBuffer_A.Add (_fBuffer_A7);


			_fBuffer_Speed_A0 = new float[_nBufferCount];
			_fBuffer_Speed_A1 = new float[_nBufferCount];
			_fBuffer_Speed_A2 = new float[_nBufferCount];
			_fBuffer_Speed_A3 = new float[_nBufferCount];
			_fBuffer_Speed_A4 = new float[_nBufferCount];
			_fBuffer_Speed_A5 = new float[_nBufferCount];
			_fBuffer_Speed_A6 = new float[_nBufferCount];
			_fBuffer_Speed_A7 = new float[_nBufferCount];

			_lstBuffer_Speed_A.Clear();
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A0);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A1);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A2);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A3);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A4);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A5);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A6);
			_lstBuffer_Speed_A.Add (_fBuffer_Speed_A7);
		}
		else
		{
			_bIsNoiseFiltering = false;
		}

		_UILblSensitivity.text = p_Sensitivity.ToString();
	}




	// 거리 조절----------------------------
	public void SetDistanceUp()
	{
		if (_Distance < 7)
			_Distance++;

		_UILblDistance.text = _Distance.ToString();
	}

	public void SetDistanceDown()
	{
		if (_Distance > 0)
			_Distance--;
		
		_UILblDistance.text = _Distance.ToString();
	}


	// 리셋 캘리브레이션----------------------
	public void ResetCalibration()
	{
		for (int i=0; i < 8; i++)
		{
			_lstCal_A[i][0] = _lstCal_A[i][1] = 512f;
		}
	}














	// Update -------------------------------------------------
	void Update () 
	{
		Update_ValueCorrection();
		Update_BufferRollingCount();

		Update_DisplayInput();
	}




	





	// 원시 데이터와 가공 데이터 0.0 ~ 1.0 -----------------------------------------
	void Update_ValueCorrection()
	{
		for (int i=0; i < 8; i++) // 아날로그 8개----
		{
			if (_bShowAnalogue[i])
			{
				_fUse_n_A[i] = (1024f - _A[i].Value) / 1024f; // 원시 값---------
				_fUse_A[i] = Each_ValueCorrection(_A[i].Value , _lstCal_A[i]); // 가공 값-------
				_fUse_A[i] = Each_NoiseFiltering(_fUse_A[i], _lstBuffer_A[i]); // 필터링 값--------
				_fUse_Speed_A[i] = Each_Speed(_fUse_A[i], _fUse_Prev_A[i]); // 스피드 값-------
				_fUse_Speed_A[i] = Each_NoiseFiltering(_fUse_Speed_A[i], _lstBuffer_Speed_A[i]); // 스피드 필터링 값--------
				_fUse_Prev_A[i] = _fUse_A[i];	// 이전값으로 쓰기 위한 값---------
			}
		}
		for (int i=0; i < 16; i++) // 디지털 16개----
		{
			if (_D[i].Value == 0)
				_nUse_D[i] ++;
			else
				_nUse_D[i] = 0;
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














	// 인풋 상황판으로 인풋정보 넘김------------------------------------------
	void Update_DisplayInput()
	{
		for (int i=0; i < 8; i++) // 아날로그 8개----
		{
			_EachGaugeA[i]._fAn = _fUse_n_A[i];
			_EachGaugeA[i]._fA = _fUse_A[i];
			_EachGaugeA[i]._fA_Speed = _fUse_Speed_A[i];
			_EachGaugeA[i]._fCal = _lstCal_A[i];
		}
		for (int i=0; i < 16; i++) // 디지털 16개----
		{
			_EachGaugeD[i]._nDigitalValue = _nUse_D[i];
		}
	}




}
