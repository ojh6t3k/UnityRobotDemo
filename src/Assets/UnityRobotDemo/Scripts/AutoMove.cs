using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour 
{
	public Vector3	_v3Ready = Vector3.zero;
	public Vector3	_v3Destination = Vector3.zero;

	public float	_fSpeed = 2f;

	bool _bIsPlay = false;


	// Start -----------------------------------------
//	void Start () 
//	{
//
//	}
	
	// Update ----------------------------------------
	void Update () 
	{
		if (!_bIsPlay)
			return;

		transform.localPosition = transform.localPosition + (_v3Destination - transform.localPosition) * Time.deltaTime * _fSpeed;

		if (Vector3.Distance(transform.localPosition, _v3Destination) < 0.01f)
			_bIsPlay = false;
	}


	// StartMove -------------------------------------
	public void StartMove()
	{
		transform.localPosition = _v3Ready;
		_bIsPlay = true;
	}

	// Reposition -----------------------------------
	public void Reposition()
	{
		transform.localPosition = _v3Ready;
		_bIsPlay = false;
	}




}
