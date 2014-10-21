using UnityEngine;
using System.Collections;



enum EMonster
{
	IDLE,
	DAMAGE,
	HIT,
	NO_HIT,
}



//public enum EMColor
//{
//	NONE,
//	WHITE,
//	RED,
//	BLUE,
//	YELLOW,
//	GREEN,
//	ORANGE,
//	PURPLE,
//}







public class Game_Color : MonoBehaviour 
{
	public Input_Correction		_input_Correction;
	
	public bool _bGamePlay = false;
	public Animation		_aniMonster;
	public Animation		_aniHandL;
	public Animation		_aniHandR;

	public UISprite			_UISprGaugeBar;
	float	_fEnemyHP = 1f;
	
	
	public UISprite			_UISprNum10;
	public UISprite			_UISprNum1;
	
	public AutoMove			_scrSuccess;
	public AutoMove			_scrTimeOut;

	public float	_fGameTime = 30f;
	float		_fTime = 0f;

	public Material		_matBall;
	public ParticleAnimator _partBeam;
	public ParticleAnimator _partHit;
	public ParticleAnimator _partSpark1;
	public ParticleAnimator _partSpark2;
	Color[] modifiedColors;

	public Material		_matMonster;
	public Texture		_texMonNormal;
	public Texture		_texMonDamage;

	public Material		_matMonsterCover;

	public GameObject		_goMonster;
	float					_fCurMonYPos = 10f;
	float					_fNextMonYPos = 10f;

	public GameObject		_goBeam;
	public GameObject		_goHitEffect;

	Color		_Color = new Color();


	bool		_bIsHit = false;


	float		_fCurMonsterR = 0f;
	float		_fCurMonsterG = 0f;
	float		_fCurMonsterB = 0f;

	float		_fMonsterR = 0f;
	float		_fMonsterG = 0f;
	float		_fMonsterB = 0f;

	//public EMColor		_ECurColor = EMColor.NONE;




	// RGB 값---------------
	float	_fColorR = 0f;
	float	_fColorG = 0f;
	float	_fColorB = 0f;


	public GameObject		_texStart;

	int		_nStep = 0;
	public	GameObject		_texStep1;
	public	GameObject		_texStep2;




	// Sound ---------------------
	public GameObject		_sndBeam;
	public GameObject		_sndSpark;
	public GameObject		_sndFireVoice;
	public GameObject		_sndMonsterDie;
	public GameObject		_sndBGMWin;
	public GameObject		_sndBGMLoss;
	public GameObject		_sndBGMColor;












	// Start -----------------------------------------------------
	void Start () 
	{
		_input_Correction.Set_BufferCount(3);
		_input_Correction.SetDistance(0f);
		PlayAni(EMonster.IDLE);
		PlayAni(EMonster.NO_HIT);
		modifiedColors = _partBeam.colorAnimation;
		ChangeMonsterColor();
	}



	// Update --------------------------------------------------
	void Update () 
	{
		if ( (_input_Correction._nUse_D[0] == 1) && (_nStep == 0) )
		{
			_nStep = 1;
			ShowStep1();
			_bGamePlay = false;
			_sndBeam.audio.Stop();
			_sndSpark.audio.Stop();
			_sndFireVoice.audio.Stop();
			_sndMonsterDie.audio.Stop();
			_sndBGMWin.audio.Stop();
			_sndBGMLoss.audio.Stop();
			_sndBGMColor.audio.Stop();
			_bIsHit = false;
			_goHitEffect.SetActive(false);
			_goBeam.SetActive(false);
			PlayAni(EMonster.NO_HIT);
			CancelInvoke("ShowStartGuide");
			HideStartGuide();
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
			RestartGame();
		}

		Update_MonsterPosition();

		if (!_bGamePlay)
			return;


		Update_CheckAni();
		Update_Time();
		Update_Output_Color();
		Update_CurrentColor();

		//Update_CurrentState();
		Update_CheckHit();
	}






	// RGB 출력----------------------------------------------------
	void Update_Output_Color()
	{
		_fColorR = _input_Correction._fUse_A[0];
		_fColorG = _input_Correction._fUse_A[1];
		_fColorB = _input_Correction._fUse_A[2];
	}


	// 현재 칼라-----------------------------------------------
	void Update_CurrentColor()
	{
		_Color = new Color(_fColorR, _fColorG, _fColorB, 1f);
		_matBall.color = _Color;
		modifiedColors[0] = _Color;
		modifiedColors[1] = _Color;
		modifiedColors[2] = _Color;
		modifiedColors[3] = _Color;
		modifiedColors[4] = _Color;
		_partBeam.colorAnimation = modifiedColors;
		_partHit.colorAnimation = modifiedColors;
		_partSpark1.colorAnimation = modifiedColors;
		_partSpark2.colorAnimation = modifiedColors;
	}


	// 현재 칼라 분류--------------------------------------------
	void Update_CurrentState()
	{
//		if (CheckColors(0.98f, 0.97f, 0.94f))
//			_ECurColor = EMColor.WHITE;
//		else if (CheckColors(0.91f, 0.12f, 0.14f))
//			_ECurColor = EMColor.RED;
//		else if (CheckColors(0.09f, 0.29f, 0.54f))
//			_ECurColor = EMColor.BLUE;
//		else if (CheckColors(0.85f, 0.86f, 0.32f))
//			_ECurColor = EMColor.YELLOW;
//		else if (CheckColors(0.21f, 0.14f, 0.28f))
//			_ECurColor = EMColor.PURPLE;
//		else if (CheckColors(0.20f, 0.56f, 0.30f))
//			_ECurColor = EMColor.GREEN;
//		else if (CheckColors(0.96f, 0.31f, 0.19f))
//			_ECurColor = EMColor.ORANGE;
//		else
//			_ECurColor = EMColor.NONE;
	}


	// Update_CheckHit -------------------------------------------------------------
	void Update_CheckHit()
	{
		if (_fCurMonYPos > 0.08f)
			return;

		_fCurMonsterR = _fCurMonsterR + (_fMonsterR - _fCurMonsterR) * Time.deltaTime;
		_fCurMonsterG = _fCurMonsterG + (_fMonsterG - _fCurMonsterG) * Time.deltaTime;
		_fCurMonsterB = _fCurMonsterB + (_fMonsterB - _fCurMonsterB) * Time.deltaTime;

		_matMonsterCover.color = new Color(_fCurMonsterR, _fCurMonsterG, _fCurMonsterB);


		if ( (CheckColors(_fCurMonsterR, _fCurMonsterG, _fCurMonsterB)) && (!_bIsHit) )  // Hit!!!!!!
		{
			_bIsHit = true;
			PlayAni(EMonster.DAMAGE);
			PlayAni(EMonster.HIT);
			_goHitEffect.SetActive(true);
			_goBeam.SetActive(true);
			_matMonster.mainTexture = _texMonDamage;
			_sndFireVoice.audio.Play();
		}
		else if ( (! CheckColors(_fCurMonsterR, _fCurMonsterG, _fCurMonsterB)) && (_bIsHit) )   // Normal
		{
			_bIsHit = false;
			_goHitEffect.SetActive(false);
			_goBeam.SetActive(false);
			_matMonster.mainTexture = _texMonNormal;
		}

		if (_bIsHit)
		{
			_fEnemyHP = _fEnemyHP - Time.deltaTime * 0.1f;
			_UISprGaugeBar.fillAmount = _fEnemyHP;

			if (!_sndBeam.audio.isPlaying)
				_sndBeam.audio.Play();
			if (!_sndSpark.audio.isPlaying)
				_sndSpark.audio.Play();



			if (_fEnemyHP <= 0f)
				GameClear();
		}
		else
		{
			_sndBeam.audio.Stop();
			_sndSpark.audio.Stop();
		}

	}




	// 칼라비교-------------------------------------------
	bool CheckColors(float p_R, float p_G, float p_B)
	{
		if ( (Mathf.Abs(_fColorR - p_R) < 0.4f) &&
		     (Mathf.Abs(_fColorG - p_G) < 0.4f) &&
		     (Mathf.Abs(_fColorB - p_B) < 0.4f) )
			return true;
		else
			return false;
	}



	void ChangeMonsterColor()
	{
		_fMonsterR = Random.Range(0f,1f);
		_fMonsterG = Random.Range(0f,1f);
		_fMonsterB = Random.Range(0f,1f);

		Invoke("ChangeMonsterColor", Random.Range(1f,5f));
	}





	// 몬스터 위치--------------------------------------------
	void Update_MonsterPosition()
	{
		if (_fCurMonYPos > _fNextMonYPos)
			_fCurMonYPos = _fCurMonYPos - Time.deltaTime * 10f;

		_goMonster.transform.localPosition = new Vector3(_goMonster.transform.localPosition.x , _fCurMonYPos, _goMonster.transform.localPosition.z);
	}





	// PlayAni ---------------------------------------------------
	void PlayAni(EMonster p_Ani)
	{
		switch(p_Ani)
		{
		case EMonster.IDLE:
			_aniMonster.CrossFade("Idle", 0.1f);
			break;
			
		case EMonster.DAMAGE:
			_aniMonster.CrossFade("Damage",0.1f);
			break;

		case EMonster.NO_HIT:
			_aniHandL.CrossFade("Idle",0.1f);
			_aniHandR.CrossFade("Idle",0.1f);
			break;

		case EMonster.HIT:
			_aniHandL.CrossFade("Hit",0.1f);
			_aniHandR.CrossFade("Hit",0.1f);
			break;


		default:
			break;
		}
	}
	
	
	
	// 애니의 완료됨을 체크 -------------------------
	void Update_CheckAni()
	{
		if (!_aniMonster.isPlaying)
		{
			if (_bIsHit)
				_aniMonster.CrossFade("Damage", 0.1f);
			else
				_aniMonster.CrossFade("Idle", 0.1f);
		}


		if (!_aniHandL.isPlaying)
		{
			if (_bIsHit)
			{
				_aniHandL.CrossFade("Hit", 0.1f);
				_aniHandR.CrossFade("Hit", 0.1f);
			}
			else
			{
				_aniHandL.CrossFade("Idle", 0.1f);
				_aniHandR.CrossFade("Idle", 0.1f);
			}
		}
	}





	// 시간 제어---------------------------------------------
	void Update_Time()
	{
		if ( (_fTime < 0f) && (_bGamePlay) )
		{
			_bGamePlay = false;
			_goBeam.SetActive(false);
			_goHitEffect.SetActive(false);
			_scrTimeOut.StartMove();
			_fTime = 0f;
			_sndBGMLoss.audio.Play();
			_sndBGMColor.audio.Stop();
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


	// 게임 클리어--------------------------------
	public void GameClear()
	{
		_bGamePlay = false;
		_goBeam.SetActive(false);
		_goHitEffect.SetActive(false);
		_scrSuccess.StartMove();
		_fNextMonYPos = -12f;
		_sndBeam.audio.Stop();
		_sndSpark.audio.Stop();
		_sndMonsterDie.audio.Play();
		_sndBGMWin.audio.Play();
		_sndBGMColor.audio.Stop();
		Invoke("ShowStartGuide", 5f);
	}








	// 리스타트 게임------------------------------
	public void RestartGame()
	{
		_fTime = _fGameTime;



		_fCurMonYPos = 10f;
		_fNextMonYPos = 0.08f;

		PlayAni(EMonster.IDLE);
		_bGamePlay = true;

		_fEnemyHP = 1f;
		_UISprGaugeBar.fillAmount = _fEnemyHP;

		PlayAni(EMonster.IDLE);
		PlayAni(EMonster.NO_HIT);

		_scrSuccess.Reposition();
		_scrTimeOut.Reposition();
		CancelInvoke("ShowStartGuide");
		HideStartGuide();


		_sndBeam.audio.Stop();
		_sndSpark.audio.Stop();
		_sndFireVoice.audio.Stop();
		_sndMonsterDie.audio.Stop();
		_sndBGMWin.audio.Stop();
		_sndBGMLoss.audio.Stop();
		_sndBGMColor.audio.Stop();



		_sndBGMColor.audio.Play();
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
