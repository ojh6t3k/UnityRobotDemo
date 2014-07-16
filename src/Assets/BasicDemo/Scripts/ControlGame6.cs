using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityRobot;
using System.Runtime.InteropServices;
using System.Text;










public class ControlGame6 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	public Input_Correction		_input_Correction;
	public GameUI				_GameUI;
	
	private static string tempPath = Application.dataPath;
	public MidiFile				_midiFile;

	[DllImport("winmm.dll")]
	private static extern int mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr handle);

	bool		_bIsPlay = false;


	List<MidiNote> _lstMidiNote = new List<MidiNote>();


	float _fMusicTime = 0f;		// float 음악 흐르는 시간---
	int _nMusicTime = 0;		// int 음악 흐르는 시간---
	int _nCurNoteCounter = 0;	// 현재 노트 카운터 --


	public GameObject		_goNoteRed;
	public GameObject		_goNoteGreen;
	public GameObject		_goNoteBlue;


	int		_nTrackNum = 1;
	public GameObject[]		_goHitBox_L = new GameObject[7];






	// Start ----------------------------------------------------------
	void Start () 
	{
		_midiFile = new MidiFile(tempPath + "/BasicDemo/Resources/Midi/snowfalls.mid");

		_lstMidiNote = _midiFile.Tracks[0].Notes;

		_input_Correction.Set_State(0.9f, 8f); // 게임 고유의 셋팅 값---------
	}




	// Update ---------------------------------------------------------
	void Update () 
	{
		if (!_bGamePlay)
			return;

		Update_Check_Button();
		Update_CheckMusicTime();
		Update_CheckNoteTiming();
	}





	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;

		_bGamePlay = p_Bool;

		if (_bGamePlay)
			PlayMidi();
	}



	// 버튼 체크------------------------
	void Update_Check_Button()
	{
		if (_input_Correction._nUse_D_L == 1)
		{
			PlayMidi();
		}
		else if (_input_Correction._nUse_D_R == 1)
		{
			StopMidi();
		}

		// DO --------------------------------------------------------------------
		if (_input_Correction._bUse_D_1)
			_goHitBox_L[0].transform.localPosition = new Vector3(0.075f, 0f, 0.6f);
		else
			_goHitBox_L[0].transform.localPosition = new Vector3(0.075f, 10f, 0.6f);
		// RE ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_2)
			_goHitBox_L[1].transform.localPosition = new Vector3(0.225f, 0f, 0.6f);
		else
			_goHitBox_L[1].transform.localPosition = new Vector3(0.225f, 10f, 0.6f);
		// MI ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_3)
			_goHitBox_L[2].transform.localPosition = new Vector3(0.375f, 0f, 0.6f);
		else
			_goHitBox_L[2].transform.localPosition = new Vector3(0.375f, 10f, 0.6f);
		// FA ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_4)
			_goHitBox_L[3].transform.localPosition = new Vector3(0.525f, 0f, 0.6f);
		else
			_goHitBox_L[3].transform.localPosition = new Vector3(0.525f, 10f, 0.6f);
		// SOL ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_5)
			_goHitBox_L[4].transform.localPosition = new Vector3(0.675f, 0f, 0.6f);
		else
			_goHitBox_L[4].transform.localPosition = new Vector3(0.675f, 10f, 0.6f);
		// LA ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_6)
			_goHitBox_L[5].transform.localPosition = new Vector3(0.825f, 0f, 0.6f);
		else
			_goHitBox_L[5].transform.localPosition = new Vector3(0.825f, 10f, 0.6f);
		// TI ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_7)
			_goHitBox_L[6].transform.localPosition = new Vector3(0.975f, 0f, 0.6f);
		else
			_goHitBox_L[6].transform.localPosition = new Vector3(0.975f, 10f, 0.6f);





	}







	// PlayMidi -------------------------------------------------------------------------------------
	public void PlayMidi()
	{
		_bIsPlay = true;

		_fMusicTime = 0f;
		_nCurNoteCounter = 0;

		string commandString = "open " + tempPath + "/BasicDemo/Resources/Midi/snowfalls.mid" + " type SEQUENCER alias MIDI";
		mciSendString(commandString, null, 0, IntPtr.Zero);
		mciSendString("seek MIDI to 0", null, 0, IntPtr.Zero);
		mciSendString("play MIDI", null, 0, IntPtr.Zero);
	}


	// StopMidi -------------------------------------------------------------------------------------
	public void StopMidi()
	{
		if(_bIsPlay)
			mciSendString("stop MIDI", null, 0, IntPtr.Zero);
		
		mciSendString("close MIDI", null, 0, IntPtr.Zero);

		_fMusicTime = 0f;
		_nCurNoteCounter = 0;

		_bGamePlay = false;
	}





	// 음악 시작시 시간 누적-----------------------------------
	void Update_CheckMusicTime()
	{
		if(!_bIsPlay)
			return;

		_fMusicTime = _fMusicTime + Time.deltaTime;
		_nMusicTime = (Int32)(_fMusicTime * 384f);
	}



	// 해당 노트 타이밍-----------------------------------------
	void Update_CheckNoteTiming()
	{
		if(!_bIsPlay)
			return;

		if (_lstMidiNote[_nCurNoteCounter].StartTime > _nMusicTime)
			return;

		Debug.Log( _lstMidiNote[_nCurNoteCounter].Number );

		if (_lstMidiNote.Count-1 == _nCurNoteCounter)
			_bIsPlay = false;

		if (_lstMidiNote.Count-1 > _nCurNoteCounter)
			_nCurNoteCounter ++;
	}



	// 종료시----------------------------------------------------------------
	void OnApplicationQuit() 
	{
		StopMidi();
	}








}





















