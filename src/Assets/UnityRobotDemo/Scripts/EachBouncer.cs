using UnityEngine;
using System.Collections;

public class EachBouncer : MonoBehaviour 
{

	float _fScale = 1f;



	// UStart -------------------------------------
	void Start () 
	{
	
	}
	
	// Update -------------------------------------
	void Update () 
	{
		if (_fScale <= 1f)
			return;

		_fScale = _fScale - Time.deltaTime * 3f;

		transform.localScale = new Vector3(_fScale, _fScale, 1f);
	}





	// OnCollisionEnter -----------------------------------
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball")
		{
			_fScale = 1.4f;

			Vector3 v3Force = other.gameObject.transform.localPosition - transform.localPosition;
			v3Force = new Vector3(v3Force.x, 0f, v3Force.z);
			v3Force.Normalize();
			other.gameObject.rigidbody.velocity = Vector3.zero;
			other.gameObject.rigidbody.AddForce(v3Force * 200f);
		}
	}


}
