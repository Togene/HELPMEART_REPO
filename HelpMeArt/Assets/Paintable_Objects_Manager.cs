using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable_Objects_Manager : MonoBehaviour {


    public DynamicPaintApplyShader[] paintApplyObjects;

    // Use this for initialization
    void Start ()
    {
        paintApplyObjects = FindObjectsOfType<DynamicPaintApplyShader>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
