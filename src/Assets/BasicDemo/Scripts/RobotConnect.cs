using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class RobotConnect : MonoBehaviour 
{
	public RobotProxy _RobotProxy;
	public Input_Correction _Input_Correction; // 인풋보정---------

	private string	_statusMessage = "Ready";
	private bool	_connecting = false; // Not use


	// DF_GUI ============
	public dfDropdown	_dfDDown;
	public dfLabel		_dfLblMessage;
	public dfButton		_dfBtn_Connect;
	public dfButton		_dfBtn_Search;
	public GameObject	_goConnectUISet;
	public dfLabel		_dfLblTitle;
	public GameObject	_goPnlTitle;
	public GameObject	_goBtn_InputDisplay;
	// DF_GUI ============

	public GameObject	_goGamePrefab;




	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_RobotProxy.OnConnected += OnConnected;
		_RobotProxy.OnConnectionFailed += OnConnectionFailed;
		_RobotProxy.OnDisconnected += OnDisconnected;
		_RobotProxy.OnSearchCompleted += OnSearchCompleted;

		_RobotProxy.PortSearch();

		if (_goGamePrefab != null)
			_dfLblTitle.Text = _goGamePrefab.name;
	}




	public void Search_Ports()
	{
		_RobotProxy.PortSearch();
	}


	public void Connect_Port()
	{
		Enable_ConnectUISet(false); // DFGUI
		_RobotProxy.portName = _dfDDown.SelectedValue;
		_RobotProxy.Connect();
		_dfLblMessage.Text = "Now connecting"; // DFGUI
	}



	



	void Enable_ConnectUISet(bool p_Bool)
	{
		_dfBtn_Connect.IsEnabled = p_Bool;
		_dfBtn_Search.IsEnabled = p_Bool;
		_dfDDown.IsEnabled = p_Bool;
	}


	void Visible_ConnectUISet(bool p_Bool)
	{
		_goConnectUISet.SetActive(p_Bool);
	}


	void Visible_GameTitle(bool p_Bool)
	{
		_goPnlTitle.SetActive(p_Bool);
	}


	public void GotoLauncher()
	{
		Application.LoadLevel ("BasicDemo_Launcher");
	}



	void StartGame()
	{
		_Input_Correction.Invoke("StartInput", 0.1f);

//		if (_goGamePrefab != null)
//			_goGamePrefab.BroadcastMessage("GamePlay", true);
//		else
//			Debug.LogWarning("No GamePrefab");
	}


	void PauseGame()
	{
		if (_goGamePrefab != null)
			_goGamePrefab.BroadcastMessage("GamePlay", false);
	}






	void OnConnected(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Success to conncet"; // DFGUI
		Visible_ConnectUISet(false); // DFGUI
		_connecting = true;
		Visible_GameTitle(false); // DFGUI
		_goBtn_InputDisplay.SetActive(true); // DFGUI
		StartGame(); // Start Game
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Failed to conncet"; // DFGUI
		Enable_ConnectUISet(true); // DFGUI
		_goBtn_InputDisplay.SetActive(false); // DFGUI
		_Input_Correction._goInputPanel.SetActive(false); // 혹시 열려있을 인풋상황창을 닫는다------
		_connecting = false;
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Disconnected"; // DFGUI
		Visible_ConnectUISet(true); // DFGUI
		Enable_ConnectUISet(true); // DFGUI
		_goBtn_InputDisplay.SetActive(false); // DFGUI
		_Input_Correction._goInputPanel.SetActive(false); // 혹시 열려있을 인풋상황창을 닫는다------
		_connecting = false;
		PauseGame(); // Pause Game

		Invoke("Clear_Ports", 0.1f);
		Invoke("Search_Ports", 0.2f);
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Search completed"; // DFGUI

		_dfDDown.Items = new string[0];

		if(_RobotProxy.portNames.Count > 0)
		{
			for(int i=0; i<_RobotProxy.portNames.Count; i++)
			{
				_dfDDown.AddItem(_RobotProxy.portNames[i]);
			}
			
			_dfDDown.SelectedIndex = 0;
		}
		else if(_RobotProxy.portNames.Count == 0)
		{
			_dfDDown.AddItem("None");
		}

		_dfLblMessage.Text = "Ready"; // DFGUI
	}
}

