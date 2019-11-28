using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Direction : MonoBehaviour
{
	public Vector3 Direction = new Vector3(0, 0, 0);
	public GameObject S1;
	public GameObject S2;
	public GameObject S3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		S1.transform.localPosition = -Direction/500;
		S2.transform.localPosition = -Direction*100/500;
		S3.transform.localPosition = -Direction*200/500;
    }
}
