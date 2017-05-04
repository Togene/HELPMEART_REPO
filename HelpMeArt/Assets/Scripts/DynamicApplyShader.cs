using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicApplyShader : MonoBehaviour {

    public Material mat; //Wraps the Shader
    public Material outPut;

    private RenderTexture buffer;
    private RenderTexture texture;
    public RenderTexture Newtexture;

    public Texture IntialTexture; 

    public float updateInterval;
    private float lastUpdateTime = 0;
    Transform viewer;

    public Color paintColor;
    private Color m_paintColor;

    // Use this for initialization
    void Start ()
    {

        outPut = GetComponent<MeshRenderer>().material;

        m_paintColor = paintColor;

        texture = new RenderTexture(1028, 1028, 16);
        viewer = Camera.main.transform;
       // mat = new Material(Shader.Find("Test/Canvas_01"));
        //mat.name = transform.name;
       // Graphics.Blit(IntialTexture, texture);
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
   
        outPut.SetTexture("_DynamicTex", texture);
    }
	
    public void UpdateTexture()
    {
        Graphics.Blit(texture, buffer, mat);
        Graphics.Blit(buffer, texture);
    }

    public void NewTexture()
    {
        Debug.Log("New Texture Made");
        //Saving the Texture

        RenderTexture temp = RenderTexture.GetTemporary(1028, 1028, 24);

        texture = new RenderTexture(1028, 1028, 16);
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);

        Graphics.Blit(null, temp, outPut);
        Graphics.Blit(temp, Newtexture);

        outPut.SetTexture("_MainTex", Newtexture);
        outPut.SetTexture("_DynamicTex", texture);

        temp.Release();
    }


	// Update is called once per frame
	void Update ()
    {
        UpdateTexture();
        mat.SetVector("_SmokeCentre", new Vector2(RayCast.texCoords.x, RayCast.texCoords.y));

        if(Paint_Collision_Detection.contantPoints.Length > 0)
        mat.SetVectorArray("_Array", Paint_Collision_Detection.contantPoints);

        mat.SetColor("_PaintColor", paintColor);

        if (!Velocity_Calculate.updateingVelocity)
        {
          // NewTexture();
          // mat.SetVector("_Transmission", new Vector4(Velocity_Calculate.DrawVelocity.x, Velocity_Calculate.DrawVelocity.y, Velocity_Calculate.DrawVelocity.z, 1.0f));
        }

        if (Input.GetMouseButton(0))
            mat.SetFloat("_DRAWMode", 1);

        if(m_paintColor != paintColor)
        {
            NewTexture();
            m_paintColor = paintColor;
        }
        else
        {
            UpdateTexture();
        }

        //if (Time.time > lastUpdateTime + updateInterval)
        //{
        //    UpdateTexture();
        //    lastUpdateTime = Time.time;
        //}
    }
}
