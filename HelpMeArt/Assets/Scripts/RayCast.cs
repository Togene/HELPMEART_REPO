using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

    public static Ray ray;
    public static Vector2 texCoords;
    public static Vector3 hitPoint;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin , ray.direction * 100);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Canvas")
            {
                //Debug.Log("Hitting");
                hitPoint = hit.point;
                texCoords = hit.textureCoord;
            }
        }
    }
}
