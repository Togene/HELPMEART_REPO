using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class line
{
   public Vector3 a, b, AB;

    public line(Vector3 _a, Vector3 _b)
    {
        a = _a;
        b = _b;

        AB = (a - b);
    }

    public void UpdateLine(Vector3 _a, Vector3 _b)
    {
        a = _a;
        b = _b;

        AB =  (a - b);
    }
}

public class Paint_Mesh_Deformer_Points : MonoBehaviour {

    public Vector3[] PaintRingTopVertices, PaintRingBottomVertices, PaintRingVerticesOrigin;
    public line[] Lines;

    public Vector3 offsetTop, offsetBottom;
    // Use this for initialization
    void Awake()
    {
        PaintRingVerticesOrigin = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];
        GetComponent<MeshFilter>().mesh.vertices.CopyTo(PaintRingVerticesOrigin, 0);


        PaintRingTopVertices = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];
        PaintRingBottomVertices = new Vector3[GetComponent<MeshFilter>().mesh.vertices.Length];

        // Transorfing vertices to World Space and saving its Original Position before Translation and Rotation
        for (int i = 0; i < PaintRingVerticesOrigin.Length; i++)
        { 
        PaintRingVerticesOrigin[i] = (transform.localToWorldMatrix) * (PaintRingVerticesOrigin[i]);
        Lines = new line[PaintRingVerticesOrigin.Length];
        }
         for (int i = 0; i < PaintRingVerticesOrigin.Length; i++)
         {
             Lines[i] = new line(PaintRingTopVertices[i], PaintRingBottomVertices[i]);
         }

        ProjectVertices();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProjectVertices();
    }


    void ProjectVertices()
    {
        for (int i = 0; i < PaintRingVerticesOrigin.Length; i++)
        {
            Vector3 PaintRingTopVerticesPosition =
                new Vector3(transform.root.position.x, transform.root.position.y, transform.root.position.z);
            //Position Handling
            PaintRingTopVertices[i] = (transform.root.localToWorldMatrix) * (PaintRingVerticesOrigin[i] + transform.localPosition + offsetTop) +
                new Vector4(PaintRingTopVerticesPosition.x, PaintRingTopVerticesPosition.y, PaintRingTopVerticesPosition.z, 0);
            //Scale Handling

            PaintRingBottomVertices[i] = (transform.root.localToWorldMatrix) * (PaintRingVerticesOrigin[i] - offsetBottom) +
                new Vector4(PaintRingTopVerticesPosition.x, PaintRingTopVerticesPosition.y, PaintRingTopVerticesPosition.z, 0);

            Lines[i] = new line(PaintRingTopVertices[i], PaintRingBottomVertices[i]);
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < PaintRingVerticesOrigin.Length; i++)
            {
                Gizmos.color = Color.green;
                Vector3 t = PaintRingTopVertices[i];
                Gizmos.DrawSphere(t, .05f);

                Gizmos.color = Color.blue;
                Vector3 b = PaintRingBottomVertices[i];
                Gizmos.DrawSphere(PaintRingBottomVertices[i], .05f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(t, b);
            }
        }
    }
}
