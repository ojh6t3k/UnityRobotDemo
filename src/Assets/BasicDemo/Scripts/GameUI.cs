using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour 
{
	public Input_Correction _Input_Correction; 

	public dfLabel	_dfl_Time;
	public dfLabel	_dfl_Score;

	bool		_IsCount = false;

	public float		_fMaxTime = 30f;
	float				_fTime = 0f;
	int					_nScore = 0;




	
	// Update -----------------------------------
	void Update () 
	{
		if(!_IsCount)
			return;

		_fTime = _fTime - Time.deltaTime;
		if (_fTime <= 30f)
			_dfl_Time.Text = string.Format("{0:f0}", _fTime);
		else 
			_dfl_Time.Text = "";

		if (_fTime < 0)
		{
			_IsCount = false;
			_Input_Correction._RobotConnect._goGamePrefab.BroadcastMessage("GamePlay", false);
		}

	}


	// 스코어 증가-----------------------
	public void AddScore()
	{
		_nScore ++;
		_dfl_Score.Text = _nScore.ToString();
	}

	public void AddScore(int p_Point)
	{
		_nScore = _nScore + p_Point;
		_dfl_Score.Text = _nScore.ToString();
	}



	// 카운터 시작-------------------------
	public void StartCounter()
	{
		_IsCount = true;
		_fTime = _fMaxTime;
		_nScore = 0;
		_dfl_Score.Text = _nScore.ToString();
	}




}
