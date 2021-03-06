﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Paint_Collision_Detection : MonoBehaviour {

    // public static List<Vector4> contantPoints = new List<Vector4>();
    public GameObject[] brushPoints;
    public static Vector4[] contantPoints = new Vector4[6];
    public Vector4[] contactPointsView = new Vector4[6];

    [Range (0, 1)]
    public float rayLength;

    public Vector3 offset;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {     
        transform.rotation = Quaternion.FromToRotation(-transform.forward, RayCast.direction) * transform.rotation;
        transform.position = RayCast.hitPoint + offset + RayCast.direction; 
        contactPointsView = contantPoints;

		for(int j = 0; j < 12; j++)
		{
        for (int i = 0; i < brushPoints.Length; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(brushPoints[i].transform.position, brushPoints[i].transform.root.forward);

            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

            if(Physics.Raycast(ray, out hit, rayLength))
            {
                //if (hit.transform.tag == "Paintable")
                //{
                    contantPoints[i % 6] = new Vector4(hit.textureCoord.x, hit.textureCoord.y, 1.0f, 1.0f);
                    //Debug.Log("Hitting");
                //}
            }
        }
		}
	}

    }
