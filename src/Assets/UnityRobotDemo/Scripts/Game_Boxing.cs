﻿using UnityEngine;
using System.Collections;






enum EAttack
{
	IDLE,
	FRONT,
	L_ZAP,
	L_POWER,
	R_ZAP,
	R_POWER,
}







public class Game_Boxing : MonoBehaviour 
{
	public Input_Correction		_input_Correction;
	
	public bool _bGamePlay = false;
	public bool	_bIsEnableAttack = true;
	public Animation		_aniBoxer;
	public Material			_matBoxer;
	public Texture			_texBoxer1;
	public Texture			_texBoxer2;
	public Texture			_texBoxer3;

	public EachBoxingEffect	_efZap;
	public EachBoxingEffect	_efPower;
	public AutoFlash		_efFlash;

	public GameObject		_goCameraCase;
	public GameObject		_goCamera;

	float	_fDamageDistance = -6f; // 데미지 순간 카메라 워크

	public UISprite			_UISprGaugeBar;
	float	_fEnemyHP = 1f;


	public UISprite			_UISprNum10;
	public UISprite			_UISprNum1;
	
	public AutoMove			_scrSuccess;
	public AutoMove			_scrTimeOut;

	public float	_fGameTime = 30f;
	float		_fTime = 0f;



	// 가속도 값---------------
	float	_fSpeedL = 0f;
	float	_fSpeedM = 0f;
	float	_fSpeedR = 0f;








	// Start ------------------------------------------------------------
	void Start () 
	{
		_input_Correction.Set_BufferCount(4);
		_input_Correction.SetDistance(7f);
		PlayAni(EAttack.IDLE);
	}





	// Update -------------------------------------------------------------
	void Update () 
	{
		if (_input_Correction._nUse_D[0] > 0)
		{
			RestartGame();
		}
		
		if (!_bGamePlay)
			return;

		Update_Output_Speed();
		Update_Attack();
		Update_CheckAni();
		Update_CameraWave();
		Update_Camera();
		Update_Time();
	}






	// 속도 출력----------------------------------------------------
	void Update_Output_Speed()
	{
		_fSpeedL = _input_Correction._fUse_Speed_A[0];
		_fSpeedM = _input_Correction._fUse_Speed_A[1];
		_fSpeedR = _input_Correction._fUse_Speed_A[2];
	}



	// 공격 체크----------------------------------------------------
	void Update_Attack()
	{
		if (!_bIsEnableAttack)
			return;

		if (_fSpeedL < -30f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.L_POWER);
			_efPower.StartBoxingEffect();
			_efFlash.StartFlash(0.01f);
			_fDamageDistance = -7f;
			_fEnemyHP = _fEnemyHP - 0.1f;
			//Debug.Log("LL");
		}
		else if (_fSpeedL < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.L_ZAP);
			_efZap.StartBoxingEffect();
			_efFlash.StartFlash(0.01f);
			_fDamageDistance = -7f;
			_fEnemyHP = _fEnemyHP - 0.03f;
			//Debug.Log("L");
		}
		else if (_fSpeedM < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.FRONT);
			_efZap.StartBoxingEffect();
			_efFlash.StartFlash(0.01f);
			_fDamageDistance = -7f;
			_fEnemyHP = _fEnemyHP - 0.03f;
			//Debug.Log("M");
		}
		else if (_fSpeedR < -30f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.R_POWER);
			_efPower.StartBoxingEffect();
			_efFlash.StartFlash(0.01f);
			_fDamageDistance = -7f;
			_fEnemyHP = _fEnemyHP - 0.1f;
			//Debug.Log("RR");
		}
		else if (_fSpeedR < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.R_ZAP);
			_efZap.StartBoxingEffect();
			_efFlash.StartFlash(0.01f);
			_fDamageDistance = -7f;
			_fEnemyHP = _fEnemyHP - 0.03f;
			//Debug.Log("R");
		}

		_UISprGaugeBar.fillAmount = _fEnemyHP;

		if ((_fEnemyHP <= 0.66f) && (_fEnemyHP > 0.33f) && (_matBoxer.mainTexture != _texBoxer2))
		{
			_matBoxer.mainTexture = _texBoxer2;
		}
		else if ((_fEnemyHP <= 0.33f) && (_matBoxer.mainTexture != _texBoxer3))
		{
			_matBoxer.mainTexture = _texBoxer3;
		}
		else if (_fEnemyHP <= 0f)
		{
			GameClear();
		}
	}




	// PlayAni ---------------------------------------------------
	void PlayAni(EAttack p_LMR)
	{
		switch(p_LMR)
		{
		case EAttack.IDLE:
			_aniBoxer.CrossFade("Idle", 1f);
			break;

		case EAttack.FRONT:
			_aniBoxer.CrossFade("Front",0.1f);
			break;

		case EAttack.L_ZAP:
			_aniBoxer.CrossFade("Left_Zap",0.1f);
			break;

		case EAttack.L_POWER:
			_aniBoxer.CrossFade("Left_Power",0.3f);
			break;
			
		case EAttack.R_ZAP:
			_aniBoxer.CrossFade("Right_Zap",0.1f);
			break;
			
		case EAttack.R_POWER:
			_aniBoxer.CrossFade("Right_Power",0.3f);
			break;

		default:
			break;
		}
	}



	// 애니의 완료됨을 체크 -------------------------
	void Update_CheckAni()
	{
		if (!_aniBoxer.isPlaying)
		{
			_bIsEnableAttack = true;
			_aniBoxer.CrossFade("Idle", 1f);
		}
	}




	// 흔들리는 카메라워크-------------------------------------------
	void Update_CameraWave()
	{
		_goCameraCase.transform.position = new Vector3(Mathf.Sin(Time.time * 4f)*0.2f, 
		                                               Mathf.Sin(Time.time * 5f)*0.2f + 100f, 
		                                               Mathf.Sin(Time.time * 6f)*0.2f);

		_goCameraCase.transform.eulerAngles = new Vector3(0f, Mathf.Sin(Time.time * 1f)*10f + 135f, 0f);
	}


	void Update_Camera()
	{
		if (_fDamageDistance < -6f)
		{
			_fDamageDistance = _fDamageDistance + (-6f - _fDamageDistance)* Time.deltaTime * 10f;
			_goCamera.transform.localPosition = new Vector3(0f, -2f, _fDamageDistance);
		}
	}



	// 시간 제어---------------------------------------------
	void Update_Time()
	{
		if (_fTime <= 0f)
		{
			_bGamePlay = false;
			_scrTimeOut.StartMove();
			_fTime = 0f;
		}

		string strTime = Mathf.Floor(_fTime).ToString();
		if (strTime.Length > 1)
		{
			_UISprNum10.spriteName = strTime.Substring(0,1);
			_UISprNum1.spriteName = strTime.Substring(1,1);
		}
		else
		{
			_UISprNum10.spriteName = "0";
			_UISprNum1.spriteName = strTime.Substring(0,1);
		}

		_fTime = _fTime - Time.deltaTime;
	}




	// 게임 클리어--------------------------------
	public void GameClear()
	{
		_bGamePlay = false;
		_scrSuccess.StartMove();
	}




	// 리스타트 게임------------------------------
	public void RestartGame()
	{
		_fTime = _fGameTime;

		PlayAni(EAttack.IDLE);
		_bGamePlay = true;

		_bIsEnableAttack = true;
		_fEnemyHP = 1f;

		_matBoxer.mainTexture = _texBoxer1;

		_scrSuccess.Reposition();
		_scrTimeOut.Reposition();
	}






}