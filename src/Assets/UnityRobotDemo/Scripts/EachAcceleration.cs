using UnityEngine;
using System.Collections;



public class EachAcceleration : MonoBehaviour 
{
	float _fTimer = 0f;

	public Material _material;
	public Texture _texRed;
	public Texture _texGreen;

	bool _bIsRed = true;

	public float _fForceX = 0;
	public float _fForceZ = 0;





	// Start --------------------------------------
	void Start () 
	{
		_material.mainTexture = _texRed;
	}





	// Update -------------------------------------
	void Update () 
	{
		if (_fTimer <= 0f)
			return;
		
		_fTimer = _fTimer - Time.deltaTime;

		if (_fTimer <= 0f)
		{
			CancelInvoke("ChangeTexture");
			_material.mainTexture = _texRed;
		}
	}


	// OnCollisionEnter -----------------------------------
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball")
		{
			_fTimer = 0.5f;

			other.gameObject.rigidbody.velocity = Vector3.zero;
			other.gameObject.rigidbody.AddForce(new Vector3(_fForceX, 0f, _fForceZ));

			CancelInvoke("ChangeTexture");
			ChangeTexture();
		}
	}


	// ChangeTexture -------------------------------------
	void ChangeTexture()
	{
		if (_bIsRed)
			_material.mainTexture = _texRed;
		else
			_material.mainTexture = _texGreen;

		_bIsRed = !_bIsRed;

		Invoke("ChangeTexture" , 0.05f);
	}










}
