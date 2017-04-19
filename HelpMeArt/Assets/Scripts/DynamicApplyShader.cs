using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicApplyShader : MonoBehaviour {

    public Material mat; //Wraps the Shader

    private RenderTexture buffer;
    private RenderTexture texture;

    public Texture IntialTexture; 

    public float updateInterval;
    private float lastUpdateTime = 0;
    Transform viewer;

    public Color paintColor;

    // Use this for initialization
    void Start ()
    {
        texture = new RenderTexture(1028, 1028, 16);
        viewer = Camera.main.transform;
       // mat = new Material(Shader.Find("Test/Canvas_01"));
        //mat.name = transform.name;
       // Graphics.Blit(IntialTexture, texture);
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
       // this.GetComponent<MeshRenderer>().material.SetTexture("_DynamicTex", texture);
    }
	
    public void UpdateTexture()
    {
        Graphics.Blit(texture, buffer, mat);
        Graphics.Blit(buffer, texture);

        this.GetComponent<MeshRenderer>().material.SetTexture("_DynamicTex", texture);
    }

	// Update is called once per frame
	void Update ()
    {
        UpdateTexture();
        mat.SetVector("_SmokeCentre", new Vector2(RayCast.texCoords.x, RayCast.texCoords.y));

        if(Collision_Detection.contantPoints.Length > 0)
        mat.SetVectorArray("_Array", Collision_Detection.contantPoints);

        mat.SetColor("_PaintColor", paintColor);
        UpdateTexture();

        //if (Input.GetMouseButton(0))
                //UpdateTexture();

        

        //if (Time.time > lastUpdateTime + updateInterval)
        //{
        //    UpdateTexture();
        //    lastUpdateTime = Time.time;
        //}
    }
}
