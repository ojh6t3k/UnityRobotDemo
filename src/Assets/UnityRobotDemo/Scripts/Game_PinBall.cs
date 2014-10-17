using UnityEngine;
using System.Collections;





public class Game_PinBall : MonoBehaviour 
{
	public Input_Correction		_input_Correction;

	public bool _bGamePlay = false;

	public GameObject		_goBase;
	public GameObject		_goColliderOpen;
	public GameObject		_goColliderClose;
	public GameObject		_goColliderClear;
	public GameObject		_goDoorL;
	public GameObject		_goDoorR;
	public GameObject		_goOpenEffect;
	public GameObject		_goBall;

	public GameObject[]		_goKey = new GameObject[4];
	public UISprite[]		_UISprKey = new UISprite[4];

	public UISprite			_UISprNum10;
	public UISprite			_UISprNum1;

	public AutoMove			_scrSuccess;
	public AutoMove			_scrTimeOut;

	public float	_fGameTime = 30f;
	float		_fTime = 0f;
	

	bool _bIsSuccess = false;


	// 밸런스 값---------------
	float	_fAngleX = 0f;
	float	_fAngleZ = 0f;
	float	_fHight = 0f;


	int _nGetKey = 0;

	public GameObject		_texStart;


	int		_nStep = 0;
	public	GameObject		_texStep1;
	public	GameObject		_texStep2;





	// Sound ---------------------
	public GameObject		_sndBGMWin;
	public GameObject		_sndBGMLoss;
	public GameObject		_sndBGMPinBall;
	public GameObject		_sndBounce;
	public GameObject		_sndSpring;
	public GameObject		_sndSlide;
	public GameObject		_sndGetKey;
	public GameObject		_sndDoor;












	// Start --------------------------------
	void Start () 
	{
		_input_Correction.Set_BufferCount(7);
		_input_Correction.SetDistance(0f);
	}



	// Update -------------------------------
	void Update () 
	{
		if ( (_input_Correction._nUse_D[0] == 1) && (_nStep == 0) )
		{
			_nStep = 1;
			ShowStep1();
			_bGamePlay = false;
			_sndBGMWin.audio.Stop();
			_sndBGMLoss.audio.Stop();
			_sndDoor.audio.Stop();
			_sndBGMPinBall.audio.Stop();
			_goBall.transform.localPosition = new Vector3(0f, -20f, 0f);
		}
		else if ( (_input_Correction._nUse_D[0] == 1) && (_nStep == 1) )
		{
			_nStep = 2;
			ShowStep2();
		}
		else if ( (_input_Correction._nUse_D[0] == 1) && (_nStep == 2) )
		{
			_nStep = 0;
			HideStep();
			_bGamePlay = true;
			RestartGame();
		}


		if (!_bGamePlay)
			return;

		Update_Time();
		Update_Output_Ballence();
		Update_Controll_Base();
		Update_BallReset();
		Update_DoorOpen();
	}




	// 밸런스 출력----------------------------------------------------
	void Update_Output_Ballence()
	{
		_fAngleX = _input_Correction._fUse_A[0] - _input_Correction._fUse_A[1];
		_fAngleZ = _input_Correction._fUse_A[2] - _input_Correction._fUse_A[3];
		_fHight = (_input_Correction._fUse_A[0] + _input_Correction._fUse_A[1] + _input_Correction._fUse_A[2] + _input_Correction._fUse_A[3]) * 0.3f;

		_goColliderOpen.transform.localPosition = Vector3.zero;
		_goColliderOpen.transform.localEulerAngles = Vector3.zero;
		_goColliderClose.transform.localPosition = Vector3.zero;
		_goColliderClose.transform.localEulerAngles = Vector3.zero;
	}

	// 게임판 컨트롤-----------------------------------------
	void Update_Controll_Base()
	{
		_goBase.transform.localEulerAngles = new Vector3(_fAngleX, 0f, _fAngleZ) * 90f;
		_goBase.transform.localPosition = new Vector3(0f, _fHight * -1f, 0f);
	}


	// 시간 제어---------------------------------------------
	void Update_Time()
	{
		if (_fTime <= 0f)
		{
			_bGamePlay = false;
			_goBall.transform.localPosition = new Vector3(0f, -20f, 0f);
			_scrTimeOut.StartMove();

			_fTime = 0f;

			_sndBGMLoss.audio.Play();
			_sndBGMPinBall.audio.Stop();

			Invoke("ShowStartGuide", 5f);
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


	// 볼 리셋-----------------------------------
	void Update_BallReset()
	{
		if(_goBall.transform.localPosition.y < -20f)
		{
			_goBall.transform.localPosition = new Vector3(0f, 6f, 0f);
			_goBall.rigidbody.velocity = Vector3.zero;
		}
	}


	// 열쇠 획득후 문열림-----------------------------
	void Update_DoorOpen()
	{
		if (_nGetKey < 4)
			return;

		if (_goDoorL.transform.localPosition.x <= -1.08f)
			return;


		if (_goDoorL.transform.localPosition.x > -1.08f)
		{
			_goDoorL.transform.localPosition = new Vector3(_goDoorL.transform.localPosition.x - Time.deltaTime, -0.04f, 0f);
			_goDoorR.transform.localPosition = new Vector3(_goDoorR.transform.localPosition.x + Time.deltaTime, -0.04f, 0f);
		}

		if (_goDoorL.transform.localPosition.x <= -1.08f)
		{
			_goOpenEffect.SetActive(true);
			_goColliderOpen.SetActive(true);
			_goColliderClose.SetActive(false);
			_goColliderClear.SetActive(true);
		}
	}

	// BallBounce ---------------------------------------
	 public void BallBounce(float p_Force)
	{
		_sndBounce.audio.volume = p_Force/7f;
		_sndBounce.audio.Play();
	}

	// BallSpring ---------------------------------
	public void BallSpring()
	{
		_sndSpring.audio.Play();
	}

	// BallSliding ------------------------------
	public void BallSliding()
	{
		_sndSlide.audio.Play();
	}


	// 열쇠 획득---------------------------------
	public void GetKey()
	{
		_nGetKey++;
		_UISprKey[_nGetKey-1].spriteName = "left_key_indecator_get";
		_sndGetKey.audio.Play();

		if (_nGetKey == 4)
			_sndDoor.audio.Play();
	}


	// 게임 클리어--------------------------------
	public void GameClear()
	{
		_bGamePlay = false;
		_goBall.transform.localPosition = new Vector3(0f, -20f, 0f);
		_goOpenEffect.SetActive(false);
		_scrSuccess.StartMove();
		_sndBGMWin.audio.Play();
		_sndBGMPinBall.audio.Stop();
		Invoke("ShowStartGuide", 5f);
	}



	// 리스타트 게임------------------------------
	public void RestartGame()
	{
		_fTime = _fGameTime;

		_goKey[0].SetActive(true);
		_goKey[1].SetActive(true);
		_goKey[2].SetActive(true);
		_goKey[3].SetActive(true);
		_nGetKey = 0;
		
		_goBall.transform.localPosition = new Vector3(0f, 6f, 0f);
		_goBall.rigidbody.velocity = Vector3.zero;

		_goDoorL.transform.localPosition = new Vector3(-0.35f, -0.04f, 0f);
		_goDoorR.transform.localPosition = new Vector3(0.35f, -0.04f, 0f);

		_goOpenEffect.SetActive(false);
		_goColliderOpen.SetActive(false);
		_goColliderClose.SetActive(true);
		_goColliderClear.SetActive(false);

		_UISprKey[0].spriteName = "left_key_indecator_empty";
		_UISprKey[1].spriteName = "left_key_indecator_empty";
		_UISprKey[2].spriteName = "left_key_indecator_empty";
		_UISprKey[3].spriteName = "left_key_indecator_empty";

		_scrSuccess.Reposition();
		_scrTimeOut.Reposition();
		CancelInvoke("ShowStartGuide");
		HideStartGuide();


		_sndBGMWin.audio.Stop();
		_sndBGMLoss.audio.Stop();
		_sndDoor.audio.Stop();
		_sndBGMPinBall.audio.Stop();

		_sndBGMPinBall.audio.Play();
	}






	void ShowStartGuide()
	{
		_scrSuccess.Reposition();
		_scrTimeOut.Reposition();
		_texStart.SetActive(true);
	}

	void HideStartGuide()
	{
		_texStart.SetActive(false);
	}



	void ShowStep1()
	{
		HideStartGuide();
		_texStep1.SetActive(true);
	}

	void ShowStep2()
	{
		_texStep1.SetActive(false);
		_texStep2.SetActive(true);
	}

	void HideStep()
	{
		_texStep1.SetActive(false);
		_texStep2.SetActive(false);
	}










}
