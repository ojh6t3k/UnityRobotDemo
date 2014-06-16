using UnityEngine;
using System.Collections;
using System;
using UnityRobot;





public class ControlGame4 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	public ADCModule		_adcForward;
	public ADCModule		_adcRight;
	public ADCModule		_adcLeft;

	float	_fadcForward = 0f;
	float	_fadcRight = 0f;
	float	_fadcLeft = 0f;

	
	public GameObject		_goCamera;
	public GameObject		_goCameraCase;

	public float			Sensitivity;
	ArrayList _buffer = new ArrayList();


	float	_fCamAng = 0f;
	float	_fForWard = 0f;


	float	_fSign = 1f;


	// Start ----------------------------------------------------------
//	void Start () 
//	{
//	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Camera();
	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		_bGamePlay = p_Bool;
	}






	// Update_Camera ----------------------------------------------------
	void Update_Camera()
	{

		_fadcForward = _adcForward.Value - (1024f - _adcForward.Value)*_adcForward.Value*0.001f ;
		_fadcRight = _adcRight.Value - (1024f - _adcRight.Value)*_adcRight.Value*0.001f ;
		_fadcLeft = _adcLeft.Value - (1024f - _adcLeft.Value)*_adcLeft.Value*0.001f ;



		_fCamAng = -((1024f - _fadcRight) - (1024f - _fadcLeft)) / 1024f * 30f;

		_fForWard = (1024f - _fadcForward) / 1024f * 20f;



		Vector2 v2Value = new Vector2(_fCamAng, _fForWard);
		Sensitivity = Mathf.Clamp(Sensitivity, 0f, 1f);
		int n = (int)((1f - Sensitivity) * 100);
		if(n > 0)
		{
			if(_buffer.Count >= n && n > 0)
				_buffer.RemoveAt(0);
			_buffer.Add(new Vector2(_fCamAng, _fForWard));
			
			if(_buffer.Count > 0)
			{
				v2Value = Vector4.zero;
				for(int i=0; i<_buffer.Count; i++)
					v2Value += (Vector2)_buffer[i];
				
				v2Value /= (float)_buffer.Count;
			}
		}
		_fCamAng = v2Value.x;
		_fForWard = v2Value.y;





		if (_fCamAng > 0)
			_fSign = 1f;
		else
			_fSign = -1f;





		_goCameraCase.transform.localEulerAngles = new Vector3(0f, _fCamAng + (_fSign * (_fForWard * Mathf.Abs(_fCamAng / 30f))), 0f);


		_goCamera.transform.localPosition = new Vector3(0f, 0f, _fForWard / 2.5f);

	}







}





















