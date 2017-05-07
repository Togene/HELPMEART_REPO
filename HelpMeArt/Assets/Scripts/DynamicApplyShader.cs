using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicApplyShader : MonoBehaviour {

    private Material paint_mat; //Wraps the Shader
    private Material outPut;

    private RenderTexture buffer;
    private RenderTexture texture;
    private RenderTexture Newtexture;

    public Texture IntialTexture;

    private Color paintColor;
    private Color m_paintColor;

    public bool paint;

    // Use this for initialization
    void Start ()
    {

        outPut = new Material(Shader.Find("Eugene/Paintable_Output"));
        outPut.name = transform.name;

        transform.GetComponent<MeshRenderer>().material = outPut;
        outPut.mainTexture = IntialTexture;
        paint_mat = new Material(Shader.Find("Eugene/Paintable"));
        paint_mat.SetFloat("_Pixels", 1028);
        paint_mat.SetVector("_Transmission", new Vector4(1, 1, 1, 1));
        paint_mat.SetFloat("_Dissipation", 0);
        paint_mat.SetFloat("_ContactPointsLength", 6);

        paint_mat.SetFloat("_SmokeRaduis", 0.02f / (transform.localScale.x / 100));

        m_paintColor = paintColor;
        texture = new RenderTexture(2056, 2056, 24);
        texture.filterMode = FilterMode.Bilinear;
        Newtexture = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
        Newtexture.filterMode = FilterMode.Bilinear; 
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
        buffer.filterMode = FilterMode.Bilinear;
        outPut.SetTexture("_DynamicTex", texture);
    }
	
    public void UpdateTexture()
    {
        Graphics.Blit(texture, buffer, paint_mat);
        Graphics.Blit(buffer, texture);
    }

    public void NewTexture()
    {
        Debug.Log("New Texture Made");
        //Saving the Texture

        RenderTexture temp = RenderTexture.GetTemporary(1028, 1028, 24);
        temp.filterMode = FilterMode.Bilinear;
        texture = new RenderTexture(1028, 1028, 16);
        texture.filterMode = FilterMode.Trilinear;
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
        buffer.filterMode = FilterMode.Trilinear; 

        Graphics.Blit(null, temp, outPut);
        Graphics.Blit(temp, Newtexture);

        outPut.SetTexture("_MainTex", Newtexture);
        outPut.SetTexture("_DynamicTex", texture);

        temp.Release();
    }


	// Update is called once per frame
	void Update ()
    {

        if (paintColor != Paint_Color.PaintColor)
        {
            paintColor = Paint_Color.PaintColor;
        }

            if (paint)
        {     
            paint_mat.SetVector("_SmokeCentre", new Vector2(RayCast.texCoords.x, RayCast.texCoords.y) / transform.localScale.x);

            if (Paint_Collision_Detection.contantPoints.Length > 0)
                paint_mat.SetVectorArray("_Array", Paint_Collision_Detection.contantPoints);

            paint_mat.SetColor("_PaintColor", paintColor);

            if (!Velocity_Calculate.updateingVelocity)
            {
                // NewTexture();
                // paint_mat.SetVector("_Transmission", new Vector4(Velocity_Calculate.DrawVelocity.x, Velocity_Calculate.DrawVelocity.y, Velocity_Calculate.DrawVelocity.z, 1.0f));
            }

            if (Input.GetMouseButton(0))
                paint_mat.SetFloat("_DRAWMode", 1);

            if (m_paintColor != paintColor)
            {
                NewTexture();
                m_paintColor = paintColor;
            }
            else
            {
                //UpdateTexture();
            }
        }

        UpdateTexture();

        paint_mat.SetFloat("_SmokeRaduis", (0.02f / (transform.localScale.x / 100)));
    }
}
