using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Color : MonoBehaviour {

    public static Color PaintColor;
    public Color colorPicker;
	// Update is called once per frame
	void Update ()
    {
        PaintColor = colorPicker;
    }
}
