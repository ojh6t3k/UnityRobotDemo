using UnityEngine;
using System.Collections;

public class NoteTranslate : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(0f, Time.deltaTime * 4f, 0f);
	}
}
