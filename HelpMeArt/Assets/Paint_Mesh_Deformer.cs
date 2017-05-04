using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Mesh_Deformer : MonoBehaviour {

    public Paint_Mesh_Deformer_Points paintPoints;
    public Vector3[] vertices, lerpedVertices;

    public float startLerpValue;
	// Use this for initialization
	void Start ()
    {
        paintPoints = FindObjectOfType<Paint_Mesh_Deformer_Points>();
        vertices = GetComponent<MeshFilter>().mesh.vertices;

        lerpedVertices = LerpVertice(startLerpValue, vertices, 0);
    }

    Vector3[] verticesToWorld()
    {
        Vector3[] vertices = GetComponent<MeshFilter>().mesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
            vertices[i] = transform.TransformPoint(vertices[i]);

        return vertices;
    }

    //Vector3 P = Vector3.Lerp(A, B, x / (A - B).Length());

    void Update ()
    {
     float Rx = (transform.root.rotation.x);
     float Rz = (transform.root.rotation.z);

    float value = g_utils.Norm(Mathf.Abs(transform.root.rotation.x), 0, 0.7f);

     Vector3[] lerpedVerticesX = LerpVertice(Rx - startLerpValue, vertices, Rx);
     Vector3[] lerpedVerticesZ = LerpVertice(Rz - startLerpValue, vertices, Rz);

        for(int i = 0; i < vertices.Length; i++)
        {
            lerpedVertices[i] = lerpedVerticesZ[i];
        }
    }

    Vector3[] LerpVertice(float value, Vector3[] verts, float direction)
    {

        Vector3[] newVerts = new Vector3[verts.Length];
        verts.CopyTo(newVerts, 0);

        for (int i = 0; i < newVerts.Length; i++)
        {

            //Bottom Right
            if (Mathf.Sign(vertices[i].x) == -1 && Mathf.Sign(vertices[i].y) == -1)
            {
                    newVerts[i] = Vector3.Lerp(paintPoints.Lines[i].b, paintPoints.Lines[i].a, value);
            }                                                                                    
            //Bottom Left                                                                        
            if (Mathf.Sign(vertices[i].x) == 1 && Mathf.Sign(vertices[i].y) == -1)               
            {
                    newVerts[i] = Vector3.Lerp(paintPoints.Lines[i].b, paintPoints.Lines[i].a, value);
            }
            //Top Left
            if (Mathf.Sign(vertices[i].x) == -1 && Mathf.Sign(vertices[i].y) == 1)
            {
                    newVerts[i] = Vector3.Lerp(paintPoints.Lines[i].a, paintPoints.Lines[i].b, value);
            }
            //Top Right
            if (Mathf.Sign(vertices[i].x) == 1 && Mathf.Sign(vertices[i].y) == 1)
            {
                    newVerts[i] = Vector3.Lerp(paintPoints.Lines[i].a, paintPoints.Lines[i].b, value);
            }

            newVerts[i] = transform.InverseTransformPoint(newVerts[i]);
        }

        return newVerts;
    }
    
    void DrawPoints()
    {
        for (int i = 0; i < paintPoints.Lines.Length; i++)
        {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), (.10f));
        }
    }

    void DrawLerpPoints()
    {
        for (int i = 0; i < lerpedVertices.Length; i++)
        {
            //Bottom Right
            if (Mathf.Sign(lerpedVertices[i].x) == -1 && Mathf.Sign(lerpedVertices[i].y) == -1)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(transform.TransformPoint(lerpedVertices[i]), new Vector3(.10f, .10f, .10f));
            }
            //Bottom Left
            if (Mathf.Sign(lerpedVertices[i].x) == 1 && Mathf.Sign(lerpedVertices[i].y) == -1)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(transform.TransformPoint(lerpedVertices[i]), new Vector3(.10f, .10f, .10f));
            }

            if (Mathf.Sign(lerpedVertices[i].x) == -1 && Mathf.Sign(lerpedVertices[i].y) == 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.TransformPoint(lerpedVertices[i]), new Vector3(.10f, .10f, .10f));
            }

            if (Mathf.Sign(lerpedVertices[i].x) == 1 && Mathf.Sign(lerpedVertices[i].y) == 1)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.TransformPoint(lerpedVertices[i]), new Vector3(.10f, .10f, .10f));
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            DrawPoints();
            DrawLerpPoints();
        }

    }
}
