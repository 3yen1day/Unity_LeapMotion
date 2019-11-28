using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Linq;
public class HandRSP : MonoBehaviour
{
	//LeapMotionの核
    private Controller controller;
    private Finger[] fingers;
    private bool[] isGripFingers;
	private bool IndexFinger;
	private float beam = 0;
	private Vector3 IndexFingerPosition = new Vector3(0f,0f,0f);
	private bool HandR = false;
	private bool SphereApper = false;
	private Vector3 FirstLocalPosition_R;
	private Vector3 FirstLocalPosition_L;
	//private Vector3 HandDirection;
	public GameObject SphereR;
	public GameObject SphereL;

	private GameObject DebugScript;
	
    void Start()
    {
		//leapMotionに接続
        controller = new Controller();
        fingers = new Finger[5];
        isGripFingers = new bool[5];
		SphereR.SetActive(false);
		SphereL.SetActive(false);

		FirstLocalPosition_R = SphereR.transform.localPosition;
		FirstLocalPosition_L = SphereL.transform.localPosition;

		DebugScript = GameObject.Find("Script");
    }


	//指の位置に球を出して、Bangで消える
    void Update()
    {
		//現在の情報入手
        Frame frame = controller.Frame();
		FingerCount(frame);

		if (IndexFinger == true)
		{
			beam += Time.deltaTime;
			//SphereがSetActiveになる
			if (beam > 0.8)
			{
				//
				//SphereR.GetComponent<R_Sphere>().IndexDirection = HandDirection;
				if (SphereApper==false)
				{
					//SphereのActiveのF→Tはこっち、T→Fは向こう
					if (HandR == true)
					{
						SphereR.SetActive(true);
						SetSphereApper(true);
					}
					else
					{
						SphereL.SetActive(true);
						SetSphereApper(true);
					}
				}

				//指定フレーム前との比較
				float NowPos = IndexFingerPosition.y;
				//Vector3 IndexDirection = HandDirection;
				Frame Beforeframe = controller.Frame(1);
				FingerCount(Beforeframe);
				float BeforePos = IndexFingerPosition.y;
				if (NowPos - BeforePos > 20)
				{
					Debug.Log("Bang!");
					if (HandR == true)
					{
						//SetSphereApper(false);
						//SphereR.GetComponent<R_Sphere>().IndexDirection = HandDirection;
						SphereApper = false;
						SphereR.GetComponent<R_Sphere>().Beam = true;
					}
					else
					{
						//SetSphereApper(false);
						//SphereR.GetComponent<R_Sphere>().IndexDirection = HandDirection;
						SphereApper = false;
						SphereL.GetComponent<L_Sphere>().Beam = true;
					}
					//ずっと人差し指伸ばしてる時のインターバル
					beam = -2.0f;
				}
			}
		}
		else
		{
			SetSphereApper(false);
			beam = 0f;
		}
	}

	//SphereのT/F制御
	void SetSphereApper(bool Apper)
	{
		SphereR.GetComponent<R_Sphere>().Apper = Apper;
		SphereL.GetComponent<L_Sphere>().Apper = Apper;
		SphereApper = Apper;
	}

	//指の情報を取得する関数
	void FingerCount(Frame frame)
	{
		if (frame.Hands.Count != 0)
		{
			//手の情報取得（List型）
			List<Hand> hand = frame.Hands;
			//ToArray()でListから配列に変換
			fingers = hand[0].Fingers.ToArray();

			//伸ばしてる指のカウント、配列の中身をboolに変換
			isGripFingers = Array.ConvertAll(fingers, new Converter<Finger, bool>(i => i.IsExtended));

			//trueのもののみカウント
			int extendedFingerCount = isGripFingers.Count(n => n == true);

			//全握りと3本以上の握りを除外
			if (extendedFingerCount == 0)
			{
				IndexFinger = false;
			}
			else if (extendedFingerCount > 2)
			{
				IndexFinger = false;
			}
			else
			{
				//人差し指が伸びてたら
				if (fingers[1].IsExtended == true)
				{
					IndexFinger = true;
					//指の角度
					//HandDirection = new Vector3(hand[0].Direction.Pitch, -1.0f * hand[0].Direction.Roll, -1.0f * hand[0].Direction.Yaw);
					//IndexFingerDirection2 = new Vector3(fingers[1].Direction.x, fingers[1].Direction.y, fingers[1].Direction.z);
					//指の位置
					IndexFingerPosition = new Vector3(fingers[1].TipPosition.x, fingers[1].TipPosition.y, fingers[1].TipPosition.z);
					//左右どっちか
					HandR = (hand[0].IsRight == true) ? true : false;
				}
				else
				{
					IndexFinger = false;
				}
			}
		}
		else
		{
			IndexFinger = false;
		}
	}


	/*
	//Sphere情報リセット関数
	void ResetSphere()
	{
		//if (!(SphereR.GetComponent<R_Sphere>().Apper))
		//{
			if (!(SphereL.GetComponent<L_Sphere>().Apper)){
				SphereL.SetActive(false);
				SphereL.GetComponent<L_Sphere>().Scale = 0;
				SphereL.transform.localPosition = FirstLocalPosition_L;
				SphereR.SetActive(false);
				SphereR.GetComponent<R_Sphere>().Scale = 0;
				SphereR.transform.localPosition = FirstLocalPosition_R;
			}
		//}
	}
	*/
}

//https://qiita.com/Hirai0827/items/bc0224393cbfa1551e8a