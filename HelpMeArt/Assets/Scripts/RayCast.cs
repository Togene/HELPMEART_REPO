using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

    public static Ray ray;
    public static Vector2 texCoords;
    public static Vector3 hitPoint;
    public static Vector3 direction;
    public GameObject obj;
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
            if (hit.transform.tag == "Paintable" && obj == null)
            {
                obj = hit.transform.gameObject;

                //Debug.Log("Hitting");
                hitPoint = hit.point;
                texCoords = hit.textureCoord;
                direction = hit.normal;

                if (obj.GetComponent<DynamicPaintApplyShader>())
                {
                    obj.GetComponent<DynamicPaintApplyShader>().paint = true;
                }

            }
            else
            {
                if (obj != null)
                {
                    obj.GetComponent<DynamicPaintApplyShader>().paint = false;
                    obj = null;
                }
            }
        }
    }
}
