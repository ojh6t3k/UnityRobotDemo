using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class RobotConnect : MonoBehaviour 
{
	public RobotProxy _RobotProxy;

	private string	_statusMessage = "Ready";
	private bool	_connecting = false;


	// DF_GUI ============
	public dfLabel		_dfLblMessage;
	public GameObject	_goIco_Connect;
	public GameObject	_goIco_Disonnect;

	// DF_GUI ============






	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_RobotProxy.OnConnected += OnConnected;
		_RobotProxy.OnConnectionFailed += OnConnectionFailed;
		_RobotProxy.OnDisconnected += OnDisconnected;
		_RobotProxy.OnSearchCompleted += OnSearchCompleted;
	}
	

	// OnGUI ------------------------------------------------------------------------------
	void OnGUI()
	{
		Rect guiRect = new Rect(10, 10, 0, 25);
		
		if(_RobotProxy.Connected == true)
		{
			guiRect.width = 100;
			if(GUI.Button(guiRect, "Disconnect") == true)
			{
				_statusMessage = "Disconnected";
				_RobotProxy.Disconnect();
			}
			guiRect.y += (guiRect.height + 5);
		}
		else
		{
			if(_connecting == false)
			{
				guiRect.width = 100;
				if(GUI.Button(guiRect, "Search") == true)
				{
					_statusMessage = "Searching...";
					_RobotProxy.PortSearch();
				}
				guiRect.y += (guiRect.height + 5);
				
				if(_RobotProxy.portNames.Count > 0)
				{
					guiRect.width = 100;
					for(int i=0; i<_RobotProxy.portNames.Count; i++)
					{
						if(GUI.Button(guiRect, _RobotProxy.portNames[i]) == true)
						{
							_statusMessage = "Connecting...";
							_connecting = true;
							_RobotProxy.portName = _RobotProxy.portNames[i];
							_RobotProxy.Connect();
						}
						guiRect.x += (100 + 5);
					}
					guiRect.x = 10;
					guiRect.y += (guiRect.height + 5);
				}
			}
		}
		
		guiRect.width = 300;
		GUI.Label(guiRect, _statusMessage);
	}




	void Change_ConnectIcon(bool p_Bool)
	{
		if (p_Bool) 
		{
			_goIco_Connect.SetActive(true);
			_goIco_Disonnect.SetActive(false);
		}
		else
		{
			_goIco_Connect.SetActive(false);
			_goIco_Disonnect.SetActive(true);
		}
	}







	void OnConnected(object sender, EventArgs e)
	{
		_statusMessage = "Success to conncet";
		_dfLblMessage.Text = "Success to conncet";
		Change_ConnectIcon(true); // DFGUI
		_connecting = true;
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_statusMessage = "Failed to conncet";
		_dfLblMessage.Text = "Failed to conncet";
		Change_ConnectIcon(false); // DFGUI
		_connecting = false;
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_statusMessage = "Disconnected";
		_dfLblMessage.Text = "Disconnected";
		Change_ConnectIcon(false); // DFGUI
		_connecting = false;
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_statusMessage = "Search completed";
		_dfLblMessage.Text = "Search completed";
	}
}

