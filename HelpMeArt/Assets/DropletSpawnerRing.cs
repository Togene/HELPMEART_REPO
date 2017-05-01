using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletSpawnerRing : MonoBehaviour {

    public int numberOfSpawners;

    public float radius, tiltAngle;

    public Paint_Droplet_Creator spawnerPrefab;

	// Use this for initialization
	void Awake ()
    {
		for(int i = 0; i < numberOfSpawners; i++)
        {
            CreateSpawner(i);
        }
	}

    public Material[] dropletMaterials;

	// Update is called once per frame
	void CreateSpawner(int index)
    {
        Transform rotater = new GameObject("Rotater").transform;
        rotater.SetParent(transform, false);
        rotater.localRotation = Quaternion.Euler(0f, index * 360f / numberOfSpawners, 0f);

        Paint_Droplet_Creator dropletSpawner = Instantiate<Paint_Droplet_Creator>(spawnerPrefab);
        dropletSpawner.dropMaterial = dropletMaterials[index % dropletMaterials.Length];
        dropletSpawner.transform.SetParent(rotater, false);
        dropletSpawner.transform.localPosition = new Vector3(0f, 0f, radius);
        dropletSpawner.transform.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}
