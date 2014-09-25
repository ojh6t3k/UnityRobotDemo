using UnityEngine;
using System.Collections;





public class EachGauge : MonoBehaviour 
{
	public UISprite _UIsprModule;

	public bool _bIsAnalog = true;

	public float _fAn = 0f; // 원시 데이터 값--
	public float _fA = 0f; // 가공된 데이터 값--
	public float _fA_Speed = 0f; // 가공된 데이터의 스피드값--

	public float[]	_fCal = {512f, 512f}; // 캘리브레이팅용 최대 최소값--


	public UILabel _UIlbl_Name;

	public UISprite _UIspr_GraphL;
	public UISprite _UIspr_GraphR;
	public Transform _tr_GraphM;


	public UILabel _UIlbl_Max;
	public UILabel _UIlbl_Min;
	public UILabel _UIlbl_Value;
	public UILabel _UIlbl_Speed;

	public int _nDigitalValue = 0;
	public UISprite _UIsprDigitalOn;





	// SetName ---------------------
	public void SetName(string p_Name)
	{
		_UIlbl_Name.text = p_Name;
	}






	// Update -------------------------------------------------------
	void Update () 
	{
		Update_Analog();
		Update_Digital();
	}



	// Update_Analog -----------------------
	void Update_Analog()
	{
		if (!_bIsAnalog)
			return;

		_UIspr_GraphL.fillAmount = _fAn;
		_UIspr_GraphR.fillAmount = _fA;

		if (_fA_Speed > 84f)
			_fA_Speed = 84;
		else if (_fA_Speed < -84f)
			_fA_Speed = -84;

		_tr_GraphM.localPosition = new Vector3(6f,_fA_Speed + 1f ,0f);

		_UIlbl_Max.text = _fCal[0].ToString();
		_UIlbl_Min.text = _fCal[1].ToString();
		_UIlbl_Value.text = string.Format("{0:f2}", _fA);
		_UIlbl_Speed.text = string.Format("{0:f0}", _fA_Speed);
	}




	// Update_Digital -----------------------
	void Update_Digital()
	{
		if (_bIsAnalog)
			return;

		if ((_nDigitalValue == 0) && (_UIsprDigitalOn.enabled))
			_UIsprDigitalOn.enabled = false;

		else if ((_nDigitalValue > 0) && (!_UIsprDigitalOn.enabled))
			_UIsprDigitalOn.enabled = true;

	}








}
