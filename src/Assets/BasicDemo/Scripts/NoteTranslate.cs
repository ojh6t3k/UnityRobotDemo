using UnityEngine;
using System.Collections;

public class NoteTranslate : MonoBehaviour 
{
	public ControlGame6	_ControlGame6;
	public GameUI		_GameUI;

	public bool			_bIsClone = false;
	public bool			_bIsSide = false;

	// Update is called once per frame
	void Update () 
	{
		Update_Position();
	}


	void Update_Position()
	{
		if ( (_ControlGame6._bIsEasy) && (_bIsClone) )
			transform.localPosition = new Vector3(transform.localPosition.x, 0.1f, transform.localPosition.z);
		else if ( (!_ControlGame6._bIsEasy) && (_bIsClone) )
			transform.localPosition = new Vector3(transform.localPosition.x, 30f, transform.localPosition.z);
		else if ( (_ControlGame6._bIsEasy) && (_bIsSide) )
			transform.localPosition = new Vector3(transform.localPosition.x, 30f, transform.localPosition.z);
		else if ( (!_ControlGame6._bIsEasy) && (_bIsSide) )
			transform.localPosition = new Vector3(transform.localPosition.x, 0.1f, transform.localPosition.z);


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
