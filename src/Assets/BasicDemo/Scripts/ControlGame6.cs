using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityRobot;
//using System.Runtime.InteropServices;
using System.Text;










public class ControlGame6 : MonoBehaviour 
{
	public bool _bGamePlay = false;

	public Input_Correction		_input_Correction;
	public GameUI				_GameUI;
	
	string tempPath;
	public MidiFile				_midiFile;

	public GameObject			_goBGM;

//	[DllImport("winmm.dll")]
//	private static extern int mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr handle);

	bool		_bIsPlay = false;
	
	public bool		_bIsEasy = true;


	List<MidiNote> _lstMidiNote = new List<MidiNote>();


	float _fMusicTime = 0f;		// float 음악 흐르는 시간---
	int _nMusicTime = 0;		// int 음악 흐르는 시간---
	int _nCurNoteCounter = 0;	// 현재 노트 카운터 --


	public Transform		_trCamera;
	Vector3		_v3CamPosL = new Vector3(-0.535f, 0.73f, -1f);
	Vector3		_v3CamPosM = new Vector3(0.525f, 0.73f, -1f);
	Vector3		_v3CamPosR = new Vector3(1.585f, 0.73f, -1f);

	Vector3		_v3CurCamPos = new Vector3(0.525f, 0.73f, -1f);



	public GameObject		_goNoteRed;
	public GameObject		_goNoteGreen;
	public GameObject		_goNoteBlue;

	public GameObject		_goTrack_L;
	public GameObject		_goTrack_M;
	public GameObject		_goTrack_R;


	int		_nTrackNum = 0;
	public GameObject[]		_goHitBox_L = new GameObject[7];
	public GameObject[]		_goHitBox_M = new GameObject[7];
	public GameObject[]		_goHitBox_R = new GameObject[7];

	public GameObject		_goHitSet_L;
	public GameObject		_goHitSet_M;
	public GameObject		_goHitSet_R;




	// Start ----------------------------------------------------------
	void Start () 
	{
		tempPath = Application.dataPath;

		_midiFile = new MidiFile(tempPath + "/BasicDemo/Resources/Midi/snowfalls.mid");

		_lstMidiNote = _midiFile.Tracks[0].Notes;

		_goTrack_L.transform.localPosition = new Vector3(-1.06f, 30f, 0f);
		_goTrack_R.transform.localPosition = new Vector3(1.06f, 30f, 0f);

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
		Update_MoveCamera();
	}





	// Game Stop(Pause) & Play ----------------------------------------
	public void GamePlay(bool p_Bool)
	{
		if (_bGamePlay == p_Bool)
			return;

		_bGamePlay = p_Bool;

		if (_bGamePlay)
			PlayBGM();
	}



	// 버튼 체크------------------------
	void Update_Check_Button()
	{
		if (_input_Correction._nUse_D_L == 1)
		{
			PlayBGM();
		}
//		else if (_input_Correction._nUse_D_R == 1)
//		{
//			//StopMidi();
//			_bIsEasy = !_bIsEasy;
//
//			if (_bIsEasy)
//			{
//				_goTrack_L.transform.localPosition = new Vector3(-1.06f, 30f, 0f);
//				_goTrack_R.transform.localPosition = new Vector3(1.06f, 30f, 0f);
//			}
//			else
//			{
//				_goTrack_L.transform.localPosition = new Vector3(-1.06f, 0f, 0f);
//				_goTrack_R.transform.localPosition = new Vector3(1.06f, 0f, 0f);
//			}
//		}

		if ((_input_Correction._fUse_A_FR > 0.9f) && (_input_Correction._fUse_A_FL < 0.9f) && (!_bIsEasy))
		{
			_v3CurCamPos = _v3CamPosL;
			_nTrackNum = -1;
		}
		else if ((_input_Correction._fUse_A_FR < 0.9f) && (_input_Correction._fUse_A_FL > 0.9f) && (!_bIsEasy))
		{
			_v3CurCamPos = _v3CamPosR;
			_nTrackNum = 1;
		}
		else
		{
			_v3CurCamPos = _v3CamPosM;
			_nTrackNum = 0;
		}




		// DO --------------------------------------------------------------------
		if (_input_Correction._bUse_D_1)
		{
			_goHitBox_L[0].transform.localPosition = new Vector3(0.075f, 0f, 0.6f);
			_goHitBox_M[0].transform.localPosition = new Vector3(0.075f, 0f, 0.6f);
			_goHitBox_R[0].transform.localPosition = new Vector3(0.075f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[0].transform.localPosition = new Vector3(0.075f, 10f, 0.6f);
			_goHitBox_M[0].transform.localPosition = new Vector3(0.075f, 10f, 0.6f);
			_goHitBox_R[0].transform.localPosition = new Vector3(0.075f, 10f, 0.6f);
		}
		// RE ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_2)
		{
			_goHitBox_L[1].transform.localPosition = new Vector3(0.225f, 0f, 0.6f);
			_goHitBox_M[1].transform.localPosition = new Vector3(0.225f, 0f, 0.6f);
			_goHitBox_R[1].transform.localPosition = new Vector3(0.225f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[1].transform.localPosition = new Vector3(0.225f, 10f, 0.6f);
			_goHitBox_M[1].transform.localPosition = new Vector3(0.225f, 10f, 0.6f);
			_goHitBox_R[1].transform.localPosition = new Vector3(0.225f, 10f, 0.6f);
		}
		// MI ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_3)
		{
			_goHitBox_L[2].transform.localPosition = new Vector3(0.375f, 0f, 0.6f);
			_goHitBox_M[2].transform.localPosition = new Vector3(0.375f, 0f, 0.6f);
			_goHitBox_R[2].transform.localPosition = new Vector3(0.375f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[2].transform.localPosition = new Vector3(0.375f, 10f, 0.6f);
			_goHitBox_M[2].transform.localPosition = new Vector3(0.375f, 10f, 0.6f);
			_goHitBox_R[2].transform.localPosition = new Vector3(0.375f, 10f, 0.6f);
		}
		// FA ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_4)
		{
			_goHitBox_L[3].transform.localPosition = new Vector3(0.525f, 0f, 0.6f);
			_goHitBox_M[3].transform.localPosition = new Vector3(0.525f, 0f, 0.6f);
			_goHitBox_R[3].transform.localPosition = new Vector3(0.525f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[3].transform.localPosition = new Vector3(0.525f, 10f, 0.6f);
			_goHitBox_M[3].transform.localPosition = new Vector3(0.525f, 10f, 0.6f);
			_goHitBox_R[3].transform.localPosition = new Vector3(0.525f, 10f, 0.6f);
		}
		// SOL ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_5)
		{
			_goHitBox_L[4].transform.localPosition = new Vector3(0.675f, 0f, 0.6f);
			_goHitBox_M[4].transform.localPosition = new Vector3(0.675f, 0f, 0.6f);
			_goHitBox_R[4].transform.localPosition = new Vector3(0.675f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[4].transform.localPosition = new Vector3(0.675f, 10f, 0.6f);
			_goHitBox_M[4].transform.localPosition = new Vector3(0.675f, 10f, 0.6f);
			_goHitBox_R[4].transform.localPosition = new Vector3(0.675f, 10f, 0.6f);
		}
		// LA ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_6)
		{
			_goHitBox_L[5].transform.localPosition = new Vector3(0.825f, 0f, 0.6f);
			_goHitBox_M[5].transform.localPosition = new Vector3(0.825f, 0f, 0.6f);
			_goHitBox_R[5].transform.localPosition = new Vector3(0.825f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[5].transform.localPosition = new Vector3(0.825f, 10f, 0.6f);
			_goHitBox_M[5].transform.localPosition = new Vector3(0.825f, 10f, 0.6f);
			_goHitBox_R[5].transform.localPosition = new Vector3(0.825f, 10f, 0.6f);
		}
		// TI ---------------------------------------------------------------------
		if (_input_Correction._bUse_D_7)
		{
			_goHitBox_L[6].transform.localPosition = new Vector3(0.975f, 0f, 0.6f);
			_goHitBox_M[6].transform.localPosition = new Vector3(0.975f, 0f, 0.6f);
			_goHitBox_R[6].transform.localPosition = new Vector3(0.975f, 0f, 0.6f);
		}
		else
		{
			_goHitBox_L[6].transform.localPosition = new Vector3(0.975f, 10f, 0.6f);
			_goHitBox_M[6].transform.localPosition = new Vector3(0.975f, 10f, 0.6f);
			_goHitBox_R[6].transform.localPosition = new Vector3(0.975f, 10f, 0.6f);
		}


		if (_nTrackNum == -1)
		{
			_goHitSet_L.transform.localPosition = new Vector3(0f, 0f, 0f);
			_goHitSet_M.transform.localPosition = new Vector3(0f, 10f, 0f);
			_goHitSet_R.transform.localPosition = new Vector3(0f, 10f, 0f);
		}
		else if (_nTrackNum == 0)
		{
			_goHitSet_L.transform.localPosition = new Vector3(0f, 10f, 0f);
			_goHitSet_M.transform.localPosition = new Vector3(0f, 0f, 0f);
			_goHitSet_R.transform.localPosition = new Vector3(0f, 10f, 0f);
		}
		else if (_nTrackNum == 1)
		{
			_goHitSet_L.transform.localPosition = new Vector3(0f, 10f, 0f);
			_goHitSet_M.transform.localPosition = new Vector3(0f, 10f, 0f);
			_goHitSet_R.transform.localPosition = new Vector3(0f, 0f, 0f);
		}



	}







	// PlayMidi -------------------------------------------------------------------------------------
	public void PlayBGM()
	{
		_bIsPlay = true;

		_fMusicTime = 0f;
		_nCurNoteCounter = 0;

		CancelInvoke("DelayPlayBGM");
		Invoke("DelayPlayBGM", 3.8f);
	}


	void DelayPlayBGM()
	{
		_goBGM.audio.Play();
//		string commandString = "open " + tempPath + "/BasicDemo/Resources/Midi/snowfalls.mid" + " type SEQUENCER alias MIDI";
//		mciSendString(commandString, null, 0, IntPtr.Zero);
//		mciSendString("seek MIDI to 0", null, 0, IntPtr.Zero);
//		mciSendString("play MIDI", null, 0, IntPtr.Zero);
	}



	// StopMidi -------------------------------------------------------------------------------------
	public void StopBGM()
	{
		if(_bIsPlay)
		{
			_goBGM.audio.Stop();
			//mciSendString("stop MIDI", null, 0, IntPtr.Zero);
		}

		//mciSendString("close MIDI", null, 0, IntPtr.Zero);

		_fMusicTime = 0f;
		_nCurNoteCounter = 0;

		CancelInvoke("DelayPlayBGM");

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

		MakeNote(_lstMidiNote[_nCurNoteCounter].Number);

		if (_lstMidiNote.Count-1 == _nCurNoteCounter)
			_bIsPlay = false;

		if (_lstMidiNote.Count-1 > _nCurNoteCounter)
			_nCurNoteCounter ++;
	}



	// 종료시----------------------------------------------------------------
	void OnApplicationQuit() 
	{
		StopBGM();
	}





	// 노트생성---------------------------------------------
	void MakeNote(int p_Int)
	{
		switch(p_Int)
		{
		case 48:
			InstantiateNote(-1, 0.075f);
			InstantiateNote(-10, 0.075f);
			break;
		case 50:
			InstantiateNote(-1, 0.225f);
			InstantiateNote(-10, 0.225f);
			break;
		case 52:
			InstantiateNote(-1, 0.375f);
			InstantiateNote(-10, 0.375f);
			break;
		case 53:
			InstantiateNote(-1, 0.525f);
			InstantiateNote(-10, 0.525f);
			break;
		case 55:
			InstantiateNote(-1, 0.675f);
			InstantiateNote(-10, 0.675f);
			break;
		case 57:
			InstantiateNote(-1, 0.825f);
			InstantiateNote(-10, 0.825f);
			break;
		case 59:
			InstantiateNote(-1, 0.975f);
			InstantiateNote(-10, 0.975f);
			break;
			
			
		case 60:
			InstantiateNote(0, 0.075f);
			break;
		case 62:
			InstantiateNote(0, 0.225f);
			break;
		case 64:
			InstantiateNote(0, 0.375f);
			break;
		case 65:
			InstantiateNote(0, 0.525f);
			break;
		case 67:
			InstantiateNote(0, 0.675f);
			break;
		case 69:
			InstantiateNote(0, 0.825f);
			break;
		case 71:
			InstantiateNote(0, 0.975f);
			break;


		case 72:
			InstantiateNote(1, 0.075f);
			InstantiateNote(10, 0.075f);
			break;
		case 74:
			InstantiateNote(1, 0.225f);
			InstantiateNote(10, 0.225f);
			break;
		case 76:
			InstantiateNote(1, 0.375f);
			InstantiateNote(10, 0.375f);
			break;
		case 77:
			InstantiateNote(1, 0.525f);
			InstantiateNote(10, 0.525f);
			break;
		case 79:
			InstantiateNote(1, 0.675f);
			InstantiateNote(10, 0.675f);
			break;
		case 81:
			InstantiateNote(1, 0.825f);
			InstantiateNote(10, 0.825f);
			break;
		case 83:
			InstantiateNote(1, 0.975f);
			InstantiateNote(10, 0.975f);
			break;


		default:
			break;
		}
	}





	void InstantiateNote(int p_Octave, float p_PositionX)
	{
		GameObject note;

		switch(p_Octave)
		{
		case -1:
			note = Instantiate(_goNoteRed) as GameObject;
			note.transform.parent = _goTrack_L.transform;
			note.transform.localPosition = new Vector3(p_PositionX, 30f, 20f);
			NoteTranslate scr = note.GetComponent<NoteTranslate>();
			scr._ControlGame6 = this;
			scr._GameUI = _GameUI;
			scr._bIsSide = true;
			break;
		case 0:
			note = Instantiate(_goNoteGreen) as GameObject;
			note.transform.parent = _goTrack_M.transform;
			note.transform.localPosition = new Vector3(p_PositionX, 0.1f, 20f);
			scr = note.GetComponent<NoteTranslate>();
			scr._ControlGame6 = this;
			scr._GameUI = _GameUI;
			break;
		case 1:
			note = Instantiate(_goNoteBlue) as GameObject;
			note.transform.parent = _goTrack_R.transform;
			note.transform.localPosition = new Vector3(p_PositionX, 30f, 20f);
			scr = note.GetComponent<NoteTranslate>();
			scr._ControlGame6 = this;
			scr._GameUI = _GameUI;
			scr._bIsSide = true;
			break;

		case -10:
			note = Instantiate(_goNoteRed) as GameObject;
			note.transform.parent = _goTrack_M.transform;
			note.transform.localPosition = new Vector3(p_PositionX, 30f, 20f);
			scr = note.GetComponent<NoteTranslate>();
			scr._ControlGame6 = this;
			scr._GameUI = _GameUI;
			scr._bIsClone = true;
			break;

		case 10:
			note = Instantiate(_goNoteBlue) as GameObject;
			note.transform.parent = _goTrack_M.transform;
			note.transform.localPosition = new Vector3(p_PositionX, 30f, 20f);
			scr = note.GetComponent<NoteTranslate>();
			scr._ControlGame6 = this;
			scr._GameUI = _GameUI;
			scr._bIsClone = true;
			break;

		default:
			break;
		}
	}







	void Update_MoveCamera()
	{
		if(Vector3.Distance(_trCamera.localPosition, _v3CurCamPos) < 0.0001f)
			return;

		_trCamera.localPosition = _trCamera.localPosition + ( _v3CurCamPos - _trCamera.localPosition) * Time.deltaTime * 10f;
	}

















}





















