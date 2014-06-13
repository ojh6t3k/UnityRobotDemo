using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class RobotConnect : MonoBehaviour 
{
	public RobotProxy _RobotProxy;

	private string	_statusMessage = "Ready";
	private bool	_connecting = false; // Not use


	// DF_GUI ============
	public GameObject	_goIco_Connect;
	public GameObject	_goIco_Disonnect;
	public dfDropdown	_dfDDown;
	public dfLabel		_dfLblMessage;
	public dfButton		_dfBtn_Connect;
	public dfButton		_dfBtn_Search;
	public GameObject	_goConnectUISet;
	public dfLabel		_dfLblTitle;
	public GameObject	_goPnlTitle;
	// DF_GUI ============

	public GameObject	_goGamePrefab;




	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_RobotProxy.OnConnected += OnConnected;
		_RobotProxy.OnConnectionFailed += OnConnectionFailed;
		_RobotProxy.OnDisconnected += OnDisconnected;
		_RobotProxy.OnSearchCompleted += OnSearchCompleted;

		Clear_Ports();
		Search_Ports();

		_dfLblTitle.Text = _goGamePrefab.name;
	}



	public void Clear_Ports()
	{
		_dfDDown.Items = new string[0];
		Search_Ports();
	}



	public void Search_Ports()
	{
		if (_dfDDown.Items.Length > 0)
			return;

		_RobotProxy.PortSearch();

		if(_RobotProxy.portNames.Count > 0)
		{
			for(int i=0; i<_RobotProxy.portNames.Count; i++)
			{
				_dfDDown.AddItem(_RobotProxy.portNames[i]);
			}

			_dfDDown.SelectedIndex = 0;
		}
		_dfLblMessage.Text = "Ready"; // DFGUI
	}


	public void Connect_Port()
	{
		Enable_ConnectUISet(false); // DFGUI
		_RobotProxy.portName = _dfDDown.SelectedValue;
		_RobotProxy.Connect();
		_dfLblMessage.Text = "Now connecting"; // DFGUI
	}


	public void Disconnect_Port()
	{
		_RobotProxy.Disconnect();
//		_dfLblMessage.Text = "Disconnected"; // DFGUI
//		Change_ConnectIcon(false); // DFGUI
//		Visible_ConnectUISet(true); // DFGUI
//		Enable_ConnectUISet(true); // DFGUI
//		_connecting = false;
	}




	void Change_ConnectIcon(bool p_Bool)
	{
		if (p_Bool) 
		{
			_goIco_Connect.SetActive(true); // DFGUI
			_goIco_Disonnect.SetActive(false); // DFGUI
		}
		else
		{
			_goIco_Connect.SetActive(false); // DFGUI
			_goIco_Disonnect.SetActive(true); // DFGUI
		}
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
		if (_goGamePrefab != null)
			_goGamePrefab.BroadcastMessage("GamePlay", true);
		else
			Debug.LogWarning("No GamePrefab");
	}


	void PauseGame()
	{
		if (_goGamePrefab != null)
			_goGamePrefab.BroadcastMessage("GamePlay", false);
	}






	void OnConnected(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Success to conncet"; // DFGUI
		Change_ConnectIcon(true); // DFGUI
		Visible_ConnectUISet(false); // DFGUI
		_connecting = true;
		Visible_GameTitle(false); // DFGUI
		StartGame(); // Start Game
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Failed to conncet"; // DFGUI
		Change_ConnectIcon(false); // DFGUI
		Enable_ConnectUISet(true); // DFGUI
		_connecting = false;
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Disconnected"; // DFGUI
		Change_ConnectIcon(false); // DFGUI
		Visible_ConnectUISet(true); // DFGUI
		Enable_ConnectUISet(true); // DFGUI
		_connecting = false;
		PauseGame(); // Pause Game

		Invoke("Clear_Ports", 0.1f);
		Invoke("Search_Ports", 0.2f);
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_dfLblMessage.Text = "Search completed"; // DFGUI
	}
}

