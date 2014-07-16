using UnityEngine;
using System.Collections;

public class NoteTranslate : MonoBehaviour 
{
	public GameUI		_GameUI;


	// Update is called once per frame
	void Update () 
	{
		transform.Translate(0f, Time.deltaTime * 5f, 0f);

		if (transform.localPosition.z < -1f)
			Destroy(gameObject);
	}


	void OnTriggerEnter()
	{
		_GameUI.AddScore();
		Destroy(gameObject);
	}

}
