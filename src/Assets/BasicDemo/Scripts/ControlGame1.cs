using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


enum EGameState
{
	READY,
	TITLE,
	PLAY,
	PAUSE,
	GAMEOVER,
}





public class ControlGame1 : MonoBehaviour 
{
	public bool _bGamePlay = false;
	EGameState	_eGameState = EGameState.READY;

	public BalanceSensor	_balanceSensor;

	public GameObject		_goMaze;
	public GameObject		_goBall;


	// Start ----------------------------------------------------------
	void Start () 
	{
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Controll_Maze();
		Update_BallReset();
	}



	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		_bGamePlay = p_Bool;
		_goBall.SetActive(p_Bool);
	}




	void Update_Controll_Maze()
	{
		if(!_balanceSensor.enabled)
			return;

		_goMaze.transform.rotation = Quaternion.AngleAxis(_balanceSensor.angle.x, Vector3.forward) * Quaternion.AngleAxis(-_balanceSensor.angle.y, Vector3.right);
		_goMaze.transform.position = Vector3.up * _balanceSensor.height;
	}



	void Update_BallReset()
	{
		if(_goBall.transform.localPosition.y < -3f)
			_goBall.transform.localPosition = new Vector3(0f, 1.6f, 0f);
	}







}





















