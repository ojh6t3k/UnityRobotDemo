using UnityEngine;
using System.Collections;
using System;
using UnityRobot;


public class BasicDemo1 : MonoBehaviour
{
	public RobotProxy robot;
	public BalanceSensor balance;
	public LightController lightController;
	public ADCModule r_color;
	public ADCModule g_color;
	public ADCModule b_color;
	public DigitalModule b_led;
	public PWMModule r_led;
	public GameObject target;
	public MeshRenderer renderer;
	public float blinkTime = 1f;

	private string _statusMessage = "Ready";
	private bool _connecting = false;
	private float _time = 0;
	private bool _toggle = true;
	Quaternion _quaternion;
	Vector3 _position;
	
	// Use this for initialization
	void Start ()
	{
		robot.OnConnected += OnConnected;
		robot.OnConnectionFailed += OnConnectionFailed;
		robot.OnDisconnected += OnDisconnected;
		robot.OnSearchCompleted += OnSearchCompleted;
		
		balance.enabled = false;
		_position = target.transform.position;
		_quaternion = target.transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(robot.Connected == true)
		{
			target.transform.rotation = _quaternion * Quaternion.AngleAxis(balance.angle.x, Vector3.forward) * Quaternion.AngleAxis(-balance.angle.y, Vector3.right);
			target.transform.position = _position + Vector3.up * balance.height;

			float r = (1024 - r_color.Value) / 1024f;
			float g = (1024 - g_color.Value) / 1024f;
			float b = (1024 - b_color.Value) / 1024f;
			renderer.sharedMaterial.color = new Color(r, g, b);

			if(_time >= blinkTime)
			{
				_time = 0;
				if(_toggle == true)
					b_led.Value = 1;
				else
					b_led.Value = 0;
				_toggle = !_toggle;
			}
			else
			{
				if(_toggle == true)
					r_led.Value = (int)(255f * _time / blinkTime);
				else
					r_led.Value = (int)(255f * (1 - _time / blinkTime));

				_time += Time.deltaTime;
			}
		}
	}
	
	void OnGUI()
	{
		Rect guiRect = new Rect(10, 10, 0, 25);
		
		if(robot.Connected == true)
		{
			guiRect.width = 100;
			if(GUI.Button(guiRect, "Disconnect") == true)
			{
				_statusMessage = "Disconnected";
				robot.Disconnect();
				balance.enabled = false;
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
					robot.PortSearch();
				}
				guiRect.y += (guiRect.height + 5);
				
				if(robot.portNames.Count > 0)
				{
					guiRect.width = 100;
					for(int i=0; i<robot.portNames.Count; i++)
					{
						if(GUI.Button(guiRect, robot.portNames[i]) == true)
						{
							_statusMessage = "Connecting...";
							_connecting = true;
							robot.portName = robot.portNames[i];
							robot.Connect();
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
	
	void OnConnected(object sender, EventArgs e)
	{
		_statusMessage = "Success to conncet";
		_connecting = false;
		balance.enabled = true;
	}
	
	void OnConnectionFailed(object sender, EventArgs e)
	{
		_statusMessage = "Failed to conncet";
		_connecting = false;
		balance.enabled = false;
	}
	
	void OnDisconnected(object sender, EventArgs e)
	{
		_statusMessage = "Disconnected";
		_connecting = false;
		balance.enabled = false;
	}
	
	void OnSearchCompleted(object sender, EventArgs e)
	{
		_statusMessage = "Search completed";
	}
}
