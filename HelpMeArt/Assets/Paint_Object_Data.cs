using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Object_Data : MonoBehaviour {

    public static Color PaintColor;
    public Color colorPicker;

	public static float PointSize;

	[Range(0, 2)]
    public float pointSize;

	// Update is called once per frame
	void Update ()
    {
        PaintColor = colorPicker;
		PointSize = pointSize;
    }
}
