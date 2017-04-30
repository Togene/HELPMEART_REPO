using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity_Calculate : MonoBehaviour
{
    public Vector3 Pos, OldPos, Velocity;
    public static Vector3 DrawVelocity;
    public static bool updateingVelocity;
    public bool updateVelCheck;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    { 
        Velocity = (transform.position - OldPos) / Time.deltaTime;
        OldPos = transform.position;

        if (Velocity.magnitude == 0)
        {
            updateingVelocity = false;
        }
        else
        {
            Debug.Log("Velocty Calculate");
            updateingVelocity = true;
        }

        updateVelCheck = updateingVelocity;
        DrawVelocity = Velocity;
    }
}
