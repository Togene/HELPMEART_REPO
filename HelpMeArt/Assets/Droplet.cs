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
       transform.LookAt(Camera.main.transform);
    }


    void OnCollisionEnter(Collision enteredCollider)
    {
        if(enteredCollider.gameObject.tag == "Paintable")
            StartCoroutine(StartReturnToPool());
    }

    void OnTriggerEnter(Collider enteredCollider)
    {
        if(enteredCollider.CompareTag("Kill Zone"))
        {
            StartCoroutine(StartReturnToPool());
        }
    }

    IEnumerator StartReturnToPool()
    {
        yield return new WaitForSeconds(0);
        ReturnToPool();
    }
}
