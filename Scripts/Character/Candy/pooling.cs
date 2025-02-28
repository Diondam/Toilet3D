using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pooling : MonoBehaviour
{
    public int poolSize = 10;
    public float spawnInterval = 1f;   // Time interval between spawns
    public float spawnRangeX = 5f;     // Range of X-axis positions
    public GameObject candy;
    public Transform spawnPoint;
    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(candy, transform, true);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
    }
    public GameObject GetPooledObject()
    {
        // Find and return an inactive object from the pool
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // If no inactive objects are found, expand the pool
        GameObject obj = Instantiate(candy);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
    private void SpawnRandomObject()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        var objectToActive = GetPooledObject();
        objectToActive.transform.position = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z); // Y and Z are kept at 0
        objectToActive.transform.rotation = new Quaternion(0,0,0,0);
        objectToActive.SetActive(true);
    }
}
