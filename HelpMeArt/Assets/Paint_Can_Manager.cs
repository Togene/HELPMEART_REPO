using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Can_Manager : MonoBehaviour {

    public GameObject paint;


    public Color paintColor;

    private Color oldpaintColor;

    public float angleActivation;

	// Use this for initialization
	void Start ()
    {
        paint.GetComponent<MeshRenderer>().material.color = paintColor;

        //for (int i = 0; i < GetComponent<MeshFilter>().mesh.vertices.Length; i++)
        //{
        //    CreateSpawner(i);
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(oldpaintColor != paintColor)
        {
            paint.GetComponent<MeshRenderer>().material.color = paintColor;
        }

		if(Mathf.Abs(transform.rotation.x) > angleActivation || Mathf.Abs(transform.rotation.z) > angleActivation)
        {
            Debug.Log("Make It Rain");
        }
	}

}
