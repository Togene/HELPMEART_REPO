using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Movement : MonoBehaviour {

    [Range(0, 10)]
    float Speed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 locomotion = new Vector3(0, 0, 0);

		if(Input.GetKey("w"))
        {
            locomotion += (transform.forward * Time.deltaTime) * Speed;
        } 
         if (Input.GetKey("s"))
        {
            locomotion += (-transform.forward * Time.deltaTime) * Speed;
        }
         if (Input.GetKey("a"))
        {
            locomotion += (-transform.right * Time.deltaTime) * Speed;
        }
         if (Input.GetKey("d"))
        {
            locomotion += (transform.right * Time.deltaTime) * Speed;
        }

        locomotion *= 0.99f;

        transform.position += locomotion;
    }
}
