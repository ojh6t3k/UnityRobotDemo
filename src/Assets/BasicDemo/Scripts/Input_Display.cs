using UnityEngine;
using System.Collections;



public class Input_Display : MonoBehaviour 
{
	// 원시 데이터 값 --------------
	public float _fA_FL_n = 0f;
	public float _fA_F_n = 0f;
	public float _fA_FR_n = 0f;
	public float _fA_U_n = 0f;
	public float _fA_D_n = 0f;
	public float _fA_L_n = 0f;
	public float _fA_R_n = 0f;
	public float _fA_P_n = 0f;

	// 가공된 데이터 값 -----------
	public float _fA_FL = 0f;
	public float _fA_F = 0f;
	public float _fA_FR = 0f;
	public float _fA_U = 0f;
	public float _fA_D = 0f;
	public float _fA_L = 0f;
	public float _fA_R = 0f;
	public float _fA_P = 0f;

	public bool _bD_1 = false;
	public bool _bD_2 = false;
	public bool _bD_3 = false;
	public bool _bD_4 = false;
	public bool _bD_5 = false;
	public bool _bD_6 = false;
	public bool _bD_7 = false;
	public bool _bD_L = false;
	public bool _bD_R = false;


	// 가공된 데이터의 스피드값------------------------
	public float	_fA_FL_Speed = 0f;
	public float	_fA_F_Speed = 0f;
	public float	_fA_FR_Speed = 0f;
	public float	_fA_U_Speed = 0f;
	public float	_fA_D_Speed = 0f;
	public float	_fA_L_Speed = 0f;
	public float	_fA_R_Speed = 0f;






	// 원시 데이터 그래프----------------
	public dfSlicedSprite	_dfss_A_FL_n;
	public dfSlicedSprite	_dfss_A_F_n;
	public dfSlicedSprite	_dfss_A_FR_n;
	public dfSlicedSprite	_dfss_A_U_n;
	public dfSlicedSprite	_dfss_A_D_n;
	public dfSlicedSprite	_dfss_A_L_n;
	public dfSlicedSprite	_dfss_A_R_n;


	// 가공된 데이터 그래프-------------  
	public dfSlicedSprite	_dfss_A_FL;
	public dfSlicedSprite	_dfss_A_F;
	public dfSlicedSprite	_dfss_A_FR;
	public dfSlicedSprite	_dfss_A_U;
	public dfSlicedSprite	_dfss_A_D;
	public dfSlicedSprite	_dfss_A_L;
	public dfSlicedSprite	_dfss_A_R;

	public dfRadialSprite	_dfrs_A_P;

	public dfSprite			_dfs_D_1;
	public dfSprite			_dfs_D_2;
	public dfSprite			_dfs_D_3;
	public dfSprite			_dfs_D_4;
	public dfSprite			_dfs_D_5;
	public dfSprite			_dfs_D_6;
	public dfSprite			_dfs_D_7;
	public dfSprite			_dfs_D_L;
	public dfSprite			_dfs_D_R;



	// 가공된 데이터의 스피드 바---------------
	public dfSprite			_dfs_A_FL_Speed;
	public dfSprite			_dfs_A_F_Speed;
	public dfSprite			_dfs_A_FR_Speed;
	public dfSprite			_dfs_A_U_Speed;
	public dfSprite			_dfs_A_D_Speed;
	public dfSprite			_dfs_A_L_Speed;
	public dfSprite			_dfs_A_R_Speed;



	// 가공된 데이터의 스피드 바 표시 레이블----------------
	public dfLabel	_dfl_A_FL_Speed;
	public dfLabel	_dfl_A_F_Speed;
	public dfLabel	_dfl_A_FR_Speed;
	public dfLabel	_dfl_A_U_Speed;
	public dfLabel	_dfl_A_D_Speed;
	public dfLabel	_dfl_A_L_Speed;
	public dfLabel	_dfl_A_R_Speed;





	// 가공된 데이터 표시 레이블 1.00 ----------------
	public dfLabel	_dfl_A_FL;
	public dfLabel	_dfl_A_F;
	public dfLabel	_dfl_A_FR;
	public dfLabel	_dfl_A_U;
	public dfLabel	_dfl_A_D;
	public dfLabel	_dfl_A_L;
	public dfLabel	_dfl_A_R;
	public dfLabel	_dfl_A_P;





	// 캘리브레이팅 최대최소값 표시 레이블-------------
	public dfLabel	_dfl_A_FL_MAX, _dfl_A_FL_MIN;
	public dfLabel	_dfl_A_F_MAX, _dfl_A_F_MIN;
	public dfLabel	_dfl_A_FR_MAX, _dfl_A_FR_MIN;
	public dfLabel	_dfl_A_U_MAX, _dfl_A_U_MIN;
	public dfLabel	_dfl_A_D_MAX, _dfl_A_D_MIN;
	public dfLabel	_dfl_A_L_MAX, _dfl_A_L_MIN;
	public dfLabel	_dfl_A_R_MAX, _dfl_A_R_MIN;



	// 현재 민감도(노이즈 보정)--------------------------
	public dfLabel	_dfl_Sensitivity;


	// 현재 민감도 (사용 거리)--------------------------
	public dfLabel	_dfl_Distance;



	// 캘리브레이팅용 최대 최소값---------------
	public float[]		_fCal_A_FL = {512f, 512f};
	public float[]		_fCal_A_F = {512f, 512f};
	public float[]		_fCal_A_FR = {512f, 512f};
	public float[]		_fCal_A_U = {512f, 512f};
	public float[]		_fCal_A_D = {512f, 512f};
	public float[]		_fCal_A_L = {512f, 512f};
	public float[]		_fCal_A_R = {512f, 512f};


	







	// Update -----------------------------------------------------------
	void Update () 
	{
		_dfss_A_FL_n.FillAmount = _fA_FL_n;
		_dfss_A_F_n.FillAmount = _fA_F_n;
		_dfss_A_FR_n.FillAmount = _fA_FR_n;
		_dfss_A_U_n.FillAmount = _fA_U_n;
		_dfss_A_D_n.FillAmount = _fA_D_n;
		_dfss_A_L_n.FillAmount = _fA_L_n;
		_dfss_A_R_n.FillAmount = _fA_R_n;


		_dfss_A_FL.FillAmount = _fA_FL;
		_dfss_A_F.FillAmount = _fA_F;
		_dfss_A_FR.FillAmount = _fA_FR;
		_dfss_A_U.FillAmount = _fA_U;
		_dfss_A_D.FillAmount = _fA_D;
		_dfss_A_L.FillAmount = _fA_L;
		_dfss_A_R.FillAmount = _fA_R;
		_dfrs_A_P.FillAmount = _fA_P * 0.75f;


		_dfs_D_1.enabled = _bD_1;
		_dfs_D_2.enabled = _bD_2;
		_dfs_D_3.enabled = _bD_3;
		_dfs_D_4.enabled = _bD_4;
		_dfs_D_5.enabled = _bD_5;
		_dfs_D_6.enabled = _bD_6;
		_dfs_D_7.enabled = _bD_7;

		_dfs_D_L.enabled = _bD_L;
		_dfs_D_R.enabled = _bD_R;





		_dfl_A_FL.Text = string.Format("{0:f2}", _fA_FL);
		_dfl_A_F.Text = string.Format("{0:f2}", _fA_F);
		_dfl_A_FR.Text = string.Format("{0:f2}", _fA_FR);
		_dfl_A_U.Text = string.Format("{0:f2}", _fA_U);
		_dfl_A_D.Text = string.Format("{0:f2}", _fA_D);
		_dfl_A_L.Text = string.Format("{0:f2}", _fA_L);
		_dfl_A_R.Text = string.Format("{0:f2}", _fA_R);
		_dfl_A_P.Text = string.Format("{0:f2}", _fA_P);



		_dfs_A_FL_Speed.RelativePosition = new Vector3(10f, 62f - _fA_FL_Speed);
		_dfs_A_F_Speed.RelativePosition = new Vector3(10f, 62f - _fA_F_Speed);
		_dfs_A_FR_Speed.RelativePosition = new Vector3(10f, 62f - _fA_FR_Speed);
		_dfs_A_U_Speed.RelativePosition = new Vector3(10f, 62f - _fA_U_Speed);
		_dfs_A_D_Speed.RelativePosition = new Vector3(10f, 62f - _fA_D_Speed);
		_dfs_A_L_Speed.RelativePosition = new Vector3(10f, 62f - _fA_L_Speed);
		_dfs_A_R_Speed.RelativePosition = new Vector3(10f, 62f - _fA_R_Speed);



		_dfl_A_FL_Speed.Text = string.Format("{0:f0}", _fA_FL_Speed);
		_dfl_A_F_Speed.Text = string.Format("{0:f0}", _fA_F_Speed);
		_dfl_A_FR_Speed.Text = string.Format("{0:f0}", _fA_FR_Speed);
		_dfl_A_U_Speed.Text = string.Format("{0:f0}", _fA_U_Speed);
		_dfl_A_D_Speed.Text = string.Format("{0:f0}", _fA_D_Speed);
		_dfl_A_L_Speed.Text = string.Format("{0:f0}", _fA_L_Speed);
		_dfl_A_R_Speed.Text = string.Format("{0:f0}", _fA_R_Speed);


		_dfl_A_FR_MAX.Text = _fCal_A_FR[1].ToString();
		_dfl_A_FR_MIN.Text = _fCal_A_FR[0].ToString();

		_dfl_A_F_MAX.Text = _fCal_A_F[1].ToString();
		_dfl_A_F_MIN.Text = _fCal_A_F[0].ToString();

		_dfl_A_FL_MAX.Text = _fCal_A_FL[1].ToString();
		_dfl_A_FL_MIN.Text = _fCal_A_FL[0].ToString();


		_dfl_A_U_MAX.Text = _fCal_A_U[1].ToString();
		_dfl_A_U_MIN.Text = _fCal_A_U[0].ToString();

		_dfl_A_D_MAX.Text = _fCal_A_D[1].ToString();
		_dfl_A_D_MIN.Text = _fCal_A_D[0].ToString();

		_dfl_A_L_MAX.Text = _fCal_A_L[1].ToString();
		_dfl_A_L_MIN.Text = _fCal_A_L[0].ToString();

		_dfl_A_R_MAX.Text = _fCal_A_R[1].ToString();
		_dfl_A_R_MIN.Text = _fCal_A_R[0].ToString();
	}


}
