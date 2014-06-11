using UnityEngine;
using System.Collections;

public class EachLauncherButton : MonoBehaviour 
{
	public bool		IsActive = true;
	public string	GameName;



	// Start -----------------------------------------
	void Start () 
	{
		dfButton button = GetComponent<dfButton> ();
		button.Text = GameName;

		button.IsEnabled = IsActive;
	}



	// GameSelect ---------------------------
	public void GameSelect()
	{
		Debug.Log ("Go " + GameName);
	}

}
