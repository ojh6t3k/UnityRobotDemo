using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour 
{

//	// Use this for initialization
//	void Start () 
//	{
//	
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//	
//	}

	public void ExitApp()
	{
		Application.Quit();
	}

	public void GotoPinBall()
	{
		Application.LoadLevel("Game_PinBall");
	}

	public void GotoSpace()
	{
		Application.LoadLevel("Game_Space");
	}

	public void GotoBoxing()
	{
		Application.LoadLevel("Game_Boxing");
	}

	public void GotoColor()
	{
		Application.LoadLevel("Game_Color");
	}

}
