using UnityEngine;
using System.Collections;

public class Game_Space : MonoBehaviour 
{
	public Input_Correction		_input_Correction;
	
	public bool _bGamePlay = false;

	public GameObject		_goCamera;
	public GameObject		_goShip;
	public GameObject		_goEffect;
	public GameObject		_goSpeed;
	public Transform		_trStoneStartPoint;
	public GameObject		_goStoneTemp;
	GameObject				_goStoneGroup;

	int		_nScore = 0;

	public UISprite			_UISprNum1000;
	public UISprite			_UISprNum100;
	public UISprite			_UISprNum10;
	public UISprite			_UISprNum1;

	float	_fFuel = 1f;

	public UISprite			_UISprFuel;

	public AutoMove			_scrGameOver;

	GameObject				_goRingsTemp;
	GameObject				_goRings;
	public Material			_matRing;
	public Material			_matFuel;

	float					_fRingColor = 0f;

	float					_fCrashTime = 0f;


	// 밸런스 값---------------
	float	_fAngleX = 0f;
	float	_fAngleY = 0f;





	// Start --------------------------------
	void Start () 
	{
		_input_Correction.Set_BufferCount(9);
		_input_Correction.SetDistance(0f);

		_goRingsTemp = Resources.Load("Prefab/Rings") as GameObject;
		//_goStoneTemp = Resources.Load("Prefab/Stone") as GameObject;
	}



	// Update ----------------------------------------
	void Update () 
	{
		if (_input_Correction._nUse_D[0] > 0)
		{
			RestartGame();
		}
		
		if (!_bGamePlay)
			return;

		Update_Output_Ballence();
		Update_Controll_SpaceShip();
		Update_UseFuel();
		Update_Crash();
	}



	// 밸런스 출력----------------------------------------------------
	void Update_Output_Ballence()
	{
		_fAngleX = _input_Correction._fUse_A[0] - _input_Correction._fUse_A[1];
		_fAngleY = _input_Correction._fUse_A[2] - _input_Correction._fUse_A[3];
	}



	// 우주선 컨트롤-----------------------------------------
	void Update_Controll_SpaceShip()
	{
		_goShip.transform.localEulerAngles = new Vector3(_fAngleX * 60f, -_fAngleY * 60f, 0f);
		_goCamera.transform.Rotate(Time.deltaTime * _fAngleX * 60f, Time.deltaTime * -_fAngleY * 60f, 0f);
		_goCamera.transform.Translate(0f, 0f, Time.deltaTime * 2f);
	}



	// 연료 소비---------------------------------------------
	void Update_UseFuel()
	{
		_fFuel = _fFuel - Time.deltaTime * 0.05f;
		_UISprFuel.fillAmount = _fFuel;

		if (_fFuel <= 0f)
		{
			_goSpeed.SetActive(false);
			_scrGameOver.StartMove();
			_goEffect.SetActive(false);
			CancelInvoke("Add_Score");
			CancelInvoke("MakeStone");
			_bGamePlay = false;
		}
	}



	//  링 밝기 변경------------------------------
	void ChangeRingColor()
	{
		if (_fRingColor >= 0.5f)
			return;

		_fRingColor = _fRingColor + 0.0005f;
		_matRing.color = new Color(_fRingColor, _fRingColor, _fRingColor, 1f);
		_matFuel.color = new Color(_fRingColor/0.5f, _fRingColor/0.5f, _fRingColor/0.5f, 1f);

		Invoke("ChangeRingColor",0.01f);
	}







	// 연료 재보충--------------------------------------
	public void RechargeFuel()
	{
		_fFuel = 1f;
	}







	// 스코어 증가------------------------------------------------------------------
	void Add_Score()
	{
		_nScore ++;

		string strScore = _nScore.ToString();

		if (strScore.Length > 3)
		{
			_UISprNum1000.spriteName = strScore.Substring(0,1);
			_UISprNum100.spriteName = strScore.Substring(1,1);
			_UISprNum10.spriteName = strScore.Substring(2,1);
			_UISprNum1.spriteName = strScore.Substring(3,1);
		}
		else if (strScore.Length > 2)
		{
			_UISprNum1000.spriteName = "0";
			_UISprNum100.spriteName = strScore.Substring(0,1);
			_UISprNum10.spriteName = strScore.Substring(1,1);
			_UISprNum1.spriteName = strScore.Substring(2,1);
		}
		else if (strScore.Length > 1)
		{
			_UISprNum1000.spriteName = "0";
			_UISprNum100.spriteName = "0";
			_UISprNum10.spriteName = strScore.Substring(0,1);
			_UISprNum1.spriteName = strScore.Substring(1,1);
		}
		else
		{
			_UISprNum1000.spriteName = "0";
			_UISprNum100.spriteName = "0";
			_UISprNum10.spriteName = "0";
			_UISprNum1.spriteName = strScore.Substring(0,1);
		}

		Invoke("Add_Score",0.1f);
	}





	// MakeStone ---------------------------------------------------------
	void MakeStone()
	{
		GameObject Stone = Instantiate(_goStoneTemp) as GameObject;
		EachStone scr = Stone.GetComponent<EachStone>();
		scr._Game_Space = this;

		Stone.transform.parent = _goStoneGroup.transform;
		Stone.transform.position = _trStoneStartPoint.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
		Stone.transform.LookAt(_goShip.transform.position);
		Invoke("MakeStone", Random.Range(1f, 5f));
	}


	// 운석충돌----------------------------------
	public void Crash()
	{
		_fCrashTime = 1f;
		_goCamera.transform.Translate(0f, 0f, -0.1f);
	}

	void Update_Crash()
	{
		if (!_bGamePlay)
			return;

		if (_fCrashTime <= 0f)
			return;

		_fCrashTime = _fCrashTime - Time.deltaTime;

		_goCamera.transform.Translate(Random.Range(-0.5f, 0.5f)*_fCrashTime, Random.Range(-0.5f, 0.5f)*_fCrashTime, Random.Range(-0.5f, 0f)*_fCrashTime);
	}





	// 리스타트 게임------------------------------
	public void RestartGame()
	{
		_goShip.transform.localEulerAngles = Vector3.zero;
		_goCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
		_goCamera.transform.localEulerAngles = Vector3.zero;
		RechargeFuel();

		_goSpeed.SetActive(true);

		Destroy(_goStoneGroup);
		_goStoneGroup = new GameObject("StoneGroup");

		Destroy(_goRings);
		_goRings = Instantiate(_goRingsTemp) as GameObject;
		
		if (!_bGamePlay)
		{
			CancelInvoke("Add_Score");
			Add_Score();

			CancelInvoke("MakeStone");
			Invoke("MakeStone", 2f);

			_scrGameOver.Reposition();
			_goEffect.SetActive(true);
		}
		else
		{
			_nScore = -1;
		}


		_fRingColor = 0f;
		_matRing.color = new Color(0f, 0f, 0f, 1f);
		_matFuel.color = new Color(0f, 0f, 0f, 1f);
		ChangeRingColor();

		_fCrashTime = 0f;

		_bGamePlay = true;
	}


}













