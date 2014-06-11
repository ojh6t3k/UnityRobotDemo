using UnityEngine;
using System.Collections;

public class EachResizingUI : MonoBehaviour 
{

	// Start ----------------------------------------------
	void Start () 
	{
		transform.localScale = Vector3.one * (Screen.width / 1920f);
	}

}
