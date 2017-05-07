using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Mesh_Deformer : MonoBehaviour {

    public Paint_Mesh_Deformer_Points paintPoints;
    public Vector3[] vertices, lerpedVertices;

    public Vector3 pivotPoint, minY, MaxY, minX, MaxX;

    public float startLerpValue, paintLerpValue;


    public float angle, offset;
	// Use this for initialization
	void Start ()
    {
        paintPoints = FindObjectOfType<Paint_Mesh_Deformer_Points>();
        vertices = GetComponent<MeshFilter>().mesh.vertices;
        lerpedVertices = new Vector3[vertices.Length];
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

        float Ry = (transform.root.rotation.y);
        float Rx = (transform.root.rotation.x);
        float Rz = (transform.root.rotation.z);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vert = vertices[i];

            if (vert.y <= minY.y)
            {
                minY = new Vector3(0, vert.y, transform.InverseTransformPoint (paintPoints.PaintRingTopVertices[i]).z);
            }

            if (vert.y >= MaxY.y)
            {
                MaxY = new Vector3(0, vert.y, transform.InverseTransformPoint(paintPoints.PaintRingTopVertices[i]).z);
            }

            if (vert.x <= minX.x)
            {
                minX = new Vector3(vert.x, 0, transform.InverseTransformPoint(paintPoints.PaintRingTopVertices[i]).z);
            }

            if (vert.x > MaxX.x)
            {
                MaxX = new Vector3(vert.x, 0, transform.InverseTransformPoint(paintPoints.PaintRingTopVertices[i]).z);
            }

        }

        pivotPoint = new Vector3(MaxX.x, MaxY.y, transform.InverseTransformPoint(paintPoints.PaintRingTopVertices[1]).z);

       pivotPoint.x = Mathf.LerpAngle(MaxX.x, minX.x, (Rz - startLerpValue));
       pivotPoint.y = Mathf.LerpAngle(MaxY.y, minY.y, (Rx - startLerpValue));

        angle = (Ry * -1.0f);

        float x1 = (pivotPoint.x * Mathf.Cos(angle) - pivotPoint.y * Mathf.Sin(angle));
        float y1 = (pivotPoint.y * Mathf.Cos(angle) + pivotPoint.x * Mathf.Sin(angle));

        pivotPoint.x = x1;
        pivotPoint.y = y1;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vert = vertices[i];

            float InverseTop = transform.InverseTransformPoint(paintPoints.PaintRingTopVertices[1]).z;
            float InverseBottom = transform.InverseTransformPoint(paintPoints.PaintRingBottomVertices[1]).z;

            float dist = (pivotPoint- vert).magnitude;

            float value = g_utils.Norm(dist, 0, 1) * 100;
  
            float zFactor = Mathf.Lerp(InverseTop, InverseBottom, value - paintLerpValue);

            lerpedVertices[i] = new Vector3(vert.x, vert.y, zFactor);
        }

        GetComponent<MeshFilter>().mesh.vertices = lerpedVertices;
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
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.TransformPoint(lerpedVertices[i]), new Vector3(.10f, .10f, .10f));
        }
    }

    void DrawGrid()
    {

        Vector3 minYPoint = transform.TransformPoint(minY);
        Vector3 MaxYPoint = transform.TransformPoint(MaxY);

        Vector3 minXPoint = transform.TransformPoint(minX);
        Vector3 MaxXPoint = transform.TransformPoint(MaxX);

        Vector3 pivotPointWorld = transform.TransformPoint(pivotPoint);

        Gizmos.DrawSphere(minYPoint, .1f);
        Gizmos.DrawSphere(MaxYPoint, .1f);
                                                           
        Gizmos.DrawSphere(minXPoint, .1f);
        Gizmos.DrawSphere(MaxXPoint, .1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(minYPoint, MaxYPoint);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(minXPoint, MaxXPoint);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pivotPointWorld, .1f);
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            //DrawPoints();
            DrawGrid();
            DrawLerpPoints();
        }

    }
}
