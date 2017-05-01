using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Droplet_Creator : MonoBehaviour {

    public float timeBewtweenSpawns;

    public Droplet[] dropletPrefabs;

    float timeSinceLastSpawn;

    public FloatRange timeBetweenSpawns, scale, randomVelocity;

    float currentSpawnDelay;

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= currentSpawnDelay)
        {
            timeSinceLastSpawn -= currentSpawnDelay;
            currentSpawnDelay = timeBetweenSpawns.RandomInRange;
            SpawnDrops();
        }
    }

    public float velocity;
    public Material dropMaterial;

    void SpawnDrops()
    {
        Droplet prefab = dropletPrefabs[Random.Range(0, dropletPrefabs.Length)];
        Droplet spawn = prefab.GetPooledInstance<Droplet>();
        spawn.transform.localPosition = transform.position;
        spawn.transform.localScale = Vector3.one * scale.RandomInRange;
        spawn.transform.localRotation = Random.rotation;

        spawn.Body.velocity = transform.up * velocity + Random.onUnitSphere * randomVelocity.RandomInRange;
        spawn.GetComponentInChildren<MeshRenderer>().material = dropMaterial;
    }
}
