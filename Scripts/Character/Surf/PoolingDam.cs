using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PoolingDam:MonoBehaviour
    {
        public GameObject objectToPool;
        public Transform[] activePosition;
        public int poolSize = 5;
        private List<GameObject> pooledObjects = new List<GameObject>();
        private Quaternion originRotate;

        public void Pooling()
        {
            originRotate = objectToPool.transform.rotation;
            // Initialize the object pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(objectToPool, transform, true);
                obj.transform.parent = transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }

            
            InvokeRepeating("InvokeActive", 0, 1);
        }

        void InvokeActive()
        {
            float time = Random.Range(0, 0.8f);
            Invoke("ActivateObject", time );
        }
        private void ActivateObject()
        {
            int randomNumber = Random.Range(0, 3);
            var objectToActive = GetPooledObject();
            objectToActive.transform.position = activePosition[randomNumber].position;
            objectToActive.transform.rotation = originRotate;
            objectToActive.SetActive(true);
        }
        
        private GameObject GetPooledObject()
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
    }
}