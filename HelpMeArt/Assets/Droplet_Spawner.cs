using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet_Spawner : MonoBehaviour {

    public float radius, tiltAngle;

    public Paint_Droplet_Creator spawnerPrefab;
    private Paint_Collision_Detection collision_detection;
    private Velocity_Calculate velocitycheck;

    // Use this for initialization
    void Awake()
    {
        collision_detection = GetComponent<Paint_Collision_Detection>();
        velocitycheck = GetComponent<Velocity_Calculate>();
        for (int i = 0; i < Paint_Collision_Detection.contantPoints.Length; i++)
        {
            CreateSpawner(i);
        }
    }

    public Material dropletMaterial;

    // Update is called once per frame
    void CreateSpawner(int index)
    {
        Paint_Droplet_Creator dropletSpawner = Instantiate<Paint_Droplet_Creator>(spawnerPrefab);
        dropletSpawner.dropMaterial = dropletMaterial;

        dropletSpawner.velocityCalc = velocitycheck;
        dropletSpawner.transform.SetParent(collision_detection.brushPoints[index].transform, false);
        dropletSpawner.transform.localPosition = new Vector3(0,0,0); 
        dropletSpawner.transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}
