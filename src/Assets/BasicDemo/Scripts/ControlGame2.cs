using UnityEngine;
using System.Collections;
using System;
using UnityRobot;





public class ControlGame2 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	public BalanceSensor	_balanceSensor;

	public GameObject		_goCamera;
	public GameObject		_goFighter;



	// Start ----------------------------------------------------------
	void Start () 
	{
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Controll_Fighter();

	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		_bGamePlay = p_Bool;
		_goFighter.SetActive(p_Bool);
	}




	void Update_Controll_Fighter()
	{
		if(!_balanceSensor.enabled)
			return;

		if (!_goFighter.activeSelf)
			return;

		_goFighter.transform.localRotation = Quaternion.AngleAxis(_balanceSensor.angle.x, Vector3.forward) * Quaternion.AngleAxis(-_balanceSensor.angle.y, Vector3.right);


	//	_goCamera.transform.rotation = Quaternion.Euler(-_balanceSensor.angle.y, 0f, _balanceSensor.angle.x);
	//	_goCamera.transform.Translate(_goCamera.transform.forward * Time.deltaTime * 3f);

	
		_goCamera.transform.Translate(0f, Time.deltaTime * _balanceSensor.angle.y * 0.1f, Time.deltaTime * 3f);
		_goCamera.transform.Rotate(0f, Time.deltaTime * -_balanceSensor.angle.x, 0f);
		_goCamera.transform.localEulerAngles = new Vector3(_goCamera.transform.localEulerAngles.x, _goCamera.transform.localEulerAngles.y, 0f);

		if (_goCamera.transform.localPosition.y < 0f)
			_goCamera.transform.localPosition = new Vector3(_goCamera.transform.localPosition.x, 0f, _goCamera.transform.localPosition.z);

	}








}





















