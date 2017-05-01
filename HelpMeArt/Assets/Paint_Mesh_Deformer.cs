using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Mesh_Deformer : MonoBehaviour {

    public Paint_Mesh_Deformer_Points paintPoints;
    public Vector3[] vertices;

	// Use this for initialization
	void Start ()
    {
        paintPoints = FindObjectOfType<Paint_Mesh_Deformer_Points>();
        vertices = GetComponent<MeshFilter>().mesh.vertices;
    }

    //Vector3 P = Vector3.Lerp(A, B, x / (A - B).Length());

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < paintPoints.Lines.Length; i++)
        {

            float x = Mathf.Abs(Mathf.Sin(transform.root.rotation.x));
            float z = Mathf.Abs(Mathf.Sin(transform.root.rotation.z));

            Debug.Log(z);

            Vector3 xPoint = Vector3.Lerp(paintPoints.Lines[i].b, paintPoints.Lines[i].a, x);
            Vector3 zPoint = Vector3.Lerp(paintPoints.Lines[i].b, paintPoints.Lines[i].a, z);

            vertices[i] = transform.InverseTransformPoint((xPoint + zPoint));

            Debug.Log((xPoint + zPoint) / 2);
        }

        GetComponent<MeshFilter>().mesh.vertices = vertices;
    }
}
