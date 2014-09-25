using UnityEngine;
using System.Collections;





public class Game_PinBall : MonoBehaviour 
{
	public Input_Correction		_input_Correction;

	public bool _bGamePlay = false;

	public GameObject		_goBase;

	// 밸런스 값---------------
	float	_fAngleX = 0f;
	float	_fAngleZ = 0f;
	float	_fHight = 0f;





	// Start --------------------------------
	void Start () 
	{
	
	}



	// Update -------------------------------
	void Update () 
	{
//		if (!_bGamePlay)
//			return;

		Update_Output_Ballence();
		Update_Controll_Base();
	}




	// 밸런스 출력----------------------------------------------------
	void Update_Output_Ballence()
	{
		_fAngleX = _input_Correction._fUse_A[0] - _input_Correction._fUse_A[1];
		_fAngleZ = _input_Correction._fUse_A[2] - _input_Correction._fUse_A[3];
		_fHight = (_input_Correction._fUse_A[0] + _input_Correction._fUse_A[1] + _input_Correction._fUse_A[2] + _input_Correction._fUse_A[3]) * 0.25f;
	}

	// 게임판 컨트롤-----------------------------------------
	void Update_Controll_Base()
	{
		_goBase.transform.localEulerAngles = new Vector3(_fAngleX, 0f, _fAngleZ) * 90f;
		_goBase.transform.localPosition = new Vector3(0f, _fHight * -1f, 0f);
	}






}
