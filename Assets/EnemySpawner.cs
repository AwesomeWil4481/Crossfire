using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject levelOneEnemy;

    public GameObject[] spawnLocations;

    void Start()
    {
        foreach (GameObject spawnLocation in spawnLocations)
        {
            SpawnEnemy(spawnLocation);
        }
    }

    public void SpawnEnemy(GameObject spawnLocation)
    {
        var s = Instantiate(levelOneEnemy, spawnLocation.transform.position, Quaternion.identity);

        s.transform.parent = gameObject.transform;
        s.GetComponent<Entity>().startLocation = spawnLocation;
        s.GetComponent<Entity>().layerMask = LayerMask.GetMask("Path") | LayerMask.GetMask("Spawn Path " + spawnLocation.name);
        s.GetComponent<Entity>().StartMovement();
    }

    void Update()
    {
        
    }
}
