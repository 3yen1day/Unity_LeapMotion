using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//指を構えた時に出現するSohere
public class R_Sphere : MonoBehaviour
{
	public bool Apper, Beam;
	//public float SphereSize = 0f;//0.005f;
	//public float AuraSize = 0f;//100f;
	//public float SphereTime = 0f;//5f;//球の初期サイズ
	public GameObject Parent;
	public Vector3 IndexDirection;
	public ParticleSystem particle2;
	public float force = 10f;

	//private float Scale = 0;
	private Rigidbody rb;
	//private float Animation;
	private float t;
	//rayの補正
	private float deltaX = 0.1f;
	private float deltaY = -0.1f;
	private Vector3 InitialPosition;
	private RaycastHit hit;
	private Ray ray;

	void Start()
	{
		Apper = false;
		rb = this.GetComponent<Rigidbody>();
		InitialPosition = this.transform.localPosition;
		IndexDirection = new Vector3(1, deltaX, deltaY);
		Debug.Log(InitialPosition);
	}

	void Update()
	{
		if (Apper == true && Beam ==false)
		{
			//particle2.Stop();
			//particle2.SetActive(false);
			//Scale += Time.deltaTime * SphereTime;
			//0なら大→小の繰り返し、1なら一回大きくなるだけ
			//Scale = (Scale > 1.0f) ? 1 : Scale;
			//Animation = (Mathf.Exp(Scale * 3f) - 1) / AuraSize + SphereSize;
			//this.transform.localScale = new Vector3(Animation, Animation, Animation);
			//this.transform.position=IndexDirection/10f;
			//Debug.Log(IndexDirection / 10f);
			rayCast();
		}
		else if (Apper == false && Beam == false)
		{
			ResetSphere();
		}


		//Sphereの発射
		if (Beam == true)
		{
			if (t == 0)
			{
				Apper = false;
				//particle2.SetActive(true);
				//Animation = (Mathf.Exp(3f) - 1) / AuraSize + SphereSize;
				//this.transform.localScale = new Vector3(Animation, Animation, Animation);
				Debug.Log("跳ぶ");
				rb.AddRelativeForce(IndexDirection * force, ForceMode.Impulse);
				this.transform.parent = null;
				t += Time.deltaTime;
				particle2.Play(true);
			}
			else if (t >= 2.0f)
			{
				t = 0;
				Beam = false;
				this.transform.parent = Parent.transform;
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				ResetSphere();

			}
			else
			{
				t += Time.deltaTime;
			}
		}
		
	}

	//rayを飛ばす
	void rayCast()
	{
		//原点、飛ばす方向
		//transform.TransformDirection(ローカルベクトル)でワールドベクトルに変換
		ray = new Ray(transform.position, transform.TransformDirection(1, deltaX,0));
		Debug.DrawRay(transform.position, transform.TransformDirection(1, deltaX , 0) * 20f,Color.red);
	}

	void ResetSphere()
	{
		//particle2.SetActive(false);
		//Scale = 0;
		this.transform.localPosition = InitialPosition;
		//Debug.Log(this.transform.localPosition);
		//this.transform.localScale = new Vector3(0, 0, 0);
		this.transform.localRotation = Quaternion.identity;

		this.gameObject.SetActive(false);
	}
}