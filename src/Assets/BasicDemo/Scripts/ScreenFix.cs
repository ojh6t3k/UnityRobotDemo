using UnityEngine;
using System.Collections;

public class ScreenFix : MonoBehaviour 
{
	void Start () 
	{
		Screen.SetResolution(1024,768,true);
	}

}
