using UnityEngine;
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

	public GameObject		_goCameraCase;
	public GameObject		_goCamera;

	float	_fDamageDistance = -6f;


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
			_fDamageDistance = -7f;
			Debug.Log("LL");
		}
		else if (_fSpeedL < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.L_ZAP);
			_fDamageDistance = -7f;
			Debug.Log("L");
		}
		else if (_fSpeedM < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.FRONT);
			_fDamageDistance = -7f;
			Debug.Log("M");
		}
		else if (_fSpeedR < -30f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.R_POWER);
			_fDamageDistance = -7f;
			Debug.Log("RR");
		}
		else if (_fSpeedR < -20f)
		{
			_bIsEnableAttack = false;
			PlayAni(EAttack.R_ZAP);
			_fDamageDistance = -7f;
			Debug.Log("R");
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









	// 리스타트 게임------------------------------
	public void RestartGame()
	{
		PlayAni(EAttack.IDLE);
		_bGamePlay = true;
	}






}
