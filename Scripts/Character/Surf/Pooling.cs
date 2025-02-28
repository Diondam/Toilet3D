using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public class Pooling : MonoBehaviour
{
    public GameObject objectToPool;
    public int poolSize = 10;
    public Transform[] activePosition;
    private List<GameObject> pooledObjects = new List<GameObject>();
    public float time;
    private Quaternion originRotate;
    private void Start()
    {
        originRotate = objectToPool.transform.rotation;
        // Initialize the object pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        
        InvokeRepeating("ActivateObject", 0f, time);
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
        GameObject obj = Instantiate(objectToPool, transform, true);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
    
    private void ActivateObject()
    {
        int randomNumber = Random.Range(0, 3);
        var objectToActive = GetPooledObject();
        objectToActive.transform.position = activePosition[randomNumber].position;
        objectToActive.transform.rotation = new Quaternion(0,0,0,0);
        objectToActive.SetActive(true);
    }
    
}
