using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Mesh_Deformer : MonoBehaviour {

    public Vector3 pivotPoint, centreWorldOrigin, centreWorld, centreTopWorldOrigin, centreTopWorld , centreFowardWorldOrigin, centreFowardWorld;

    public Vector3[] topVerticesOrigin, bottomVerticesOrigin, topVertices, bottomVertices;

    Paint_Mesh_Deformer_Points meshPoints;

	// Use this for initialization
	void Awake ()
    {
        meshPoints = transform.root.GetComponentInChildren<Paint_Mesh_Deformer_Points>();
        centreWorldOrigin = transform.TransformPoint(pivotPoint) - new Vector3(0, 1f, 0);

        centreFowardWorldOrigin = centreWorldOrigin;

        centreTopWorldOrigin = transform.TransformPoint(pivotPoint);



        topVerticesOrigin = new Vector3[meshPoints.PaintRingTopVertices.Length];
        bottomVerticesOrigin = new Vector3[meshPoints.PaintRingBottomVertices.Length];

        topVertices = new Vector3[meshPoints.PaintRingTopVertices.Length];
        bottomVertices = new Vector3[meshPoints.PaintRingBottomVertices.Length];

        meshPoints.PaintRingTopVertices.CopyTo(topVerticesOrigin, 0);
        meshPoints.PaintRingBottomVertices.CopyTo(bottomVerticesOrigin, 0);

        for(int i = 0; i < topVerticesOrigin.Length; i++)
        {
            topVerticesOrigin[i] = meshPoints.PaintRingTopVertices[i];
            bottomVerticesOrigin[i] = meshPoints.PaintRingBottomVertices[i];
        }


    }

    void Update ()
    {
        centreWorld = centreWorldOrigin + transform.position;
        centreTopWorld = centreTopWorldOrigin + transform.position;

        centreFowardWorld = centreWorld * 2;

        meshPoints.PaintRingTopVertices.CopyTo(topVertices, 0);
        meshPoints.PaintRingBottomVertices.CopyTo(bottomVertices, 0);
    }

    void DrawCentrePoint()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centreWorld, .1f);
        Gizmos.DrawSphere(centreTopWorld, .1f);
        Gizmos.DrawSphere(centreFowardWorld, .1f);
    }

    void DrawPoints()
    {
        for (int i = 0; i < topVerticesOrigin.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(topVerticesOrigin[i], .08f);
            Gizmos.DrawSphere(bottomVerticesOrigin[i], .08f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topVertices[i], .08f);
            Gizmos.DrawSphere(bottomVertices[i], .08f);


        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            DrawPoints();
            DrawCentrePoint();
        }

    }
}
