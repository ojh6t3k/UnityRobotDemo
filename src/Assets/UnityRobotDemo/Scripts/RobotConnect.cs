using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class RobotConnect : MonoBehaviour 
{
	public RobotProxy _RobotProxy;
	public Input_Correction	_Input_Correction;

	public bool	_connecting = false;

	public UIPopupList	_UIPortList;
	public GameObject	_goPopPortList;
	public GameObject	_goBtnConnect;
	public GameObject	_goBtnSearch;
	public GameObject	_goLoading;

	public GameObject	_goBtnInfo;
	public GameObject	_goInfoMonitor;

	public GameObject	_goGamePrefab;



	public void GotoLauncher()
	{
		Application.LoadLevel("Launcher");
	}




	// Start ------------------------------------------------------------------------------
	void Start ()
	{
		_RobotProxy.OnConnected += OnConnected;
		_RobotProxy.OnConnectionFailed += OnConnectionFailed;
		_RobotProxy.OnDisconnected += OnDisconnected;
		_RobotProxy.OnSearchCompleted += OnSearchCompleted;

		_RobotProxy.PortSearch();
	}




	public void ShowInfoMonitor()
	{
		_goBtnInfo.SetActive(false);
		_goInfoMonitor.SetActive(true);
	}
	public void HideInfoMonitor()
	{
		_goBtnInfo.SetActive(true);
		_goInfoMonitor.SetActive(false);
	}





	public void Search_Ports()
	{
		_RobotProxy.PortSearch();
	}


	public void Connect_Port()
	{
		_RobotProxy.portName =  _UIPortList.value;
		_RobotProxy.Connect();
		_goPopPortList.SetActive(true);		// NGUI
		_UIPortList.enabled = false;		// NGUI
		_goBtnConnect.SetActive(false);		// NGUI
		_goBtnSearch.SetActive(false);		// NGUI
		_goLoading.SetActive(true);			// NGUI
		_goBtnInfo.SetActive(false);		// NGUI
	}



	void Connecting()
	{
		_connecting = true;
	}





	void OnConnected(object sender, EventArgs e)
	{
		Invoke("Connecting", 0.5f);
		_Input_Correction.Invoke("ResetCalibration", 1f);
		_goPopPortList.SetActive(false);	// NGUI
		_UIPortList.enabled = true;			// NGUI
		_goBtnConnect.SetActive(false);		// NGUI
		_goBtnSearch.SetActive(false);		// NGUI
		_goLoading.SetActive(false);		// NGUI
		_goBtnInfo.SetActive(true);			// NGUI
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_connecting = false;
		_goPopPortList.SetActive(true);		// NGUI
		_UIPortList.enabled = true;			// NGUI
		_goBtnConnect.SetActive(true);		// NGUI
		_goBtnSearch.SetActive(true);		// NGUI
		_goLoading.SetActive(false);		// NGUI
		_goBtnInfo.SetActive(false);		// NGUI
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_connecting = false;
		_goPopPortList.SetActive(true);		// NGUI
		_UIPortList.enabled = true;			// NGUI
		_goBtnConnect.SetActive(true);		// NGUI
		_goBtnSearch.SetActive(true);		// NGUI
		_goLoading.SetActive(false);		// NGUI
		_goBtnInfo.SetActive(false);		// NGUI
		_goInfoMonitor.SetActive(false);	// NGUI
		
		Invoke("Clear_Ports", 0.1f);
		Invoke("Search_Ports", 0.2f);
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_UIPortList.items.Clear();
		
		if(_RobotProxy.portNames.Count > 0)
		{
			for(int i=0; i<_RobotProxy.portNames.Count; i++)
			{
				_UIPortList.items.Add(_RobotProxy.portNames[i]);
			}
			_UIPortList.value = _UIPortList.items[0];
			_goPopPortList.SetActive(true);		// NGUI
			_UIPortList.enabled = true;			// NGUI
			_goBtnConnect.SetActive(true);		// NGUI
			_goBtnSearch.SetActive(true);		// NGUI
			_goLoading.SetActive(false);		// NGUI
		}
		else if(_RobotProxy.portNames.Count == 0)
		{
			_UIPortList.items.Add("None");
		}
	}
}

