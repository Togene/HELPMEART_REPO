using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Droplet : PooledObject
{
    public Rigidbody Body { get; private set; }
	// Use this for initialization
	void Awake ()
    {
        Body = GetComponent<Rigidbody>();
    }

    void Update()
    {
       // transform.LookAt(Camera.main.transform);
    }


    void OnCollisionEnter(Collision enteredCollider)
    {
            ReturnToPool();
    }

    void OnTriggerEnter(Collider enteredCollider)
    {
        if(enteredCollider.CompareTag("Kill Zone"))
        {
            ReturnToPool();
        }
    }
}
