namespace Unicorn.Unicorn.Scripts.Utils
{
    public class Pooling
    {
        // public void Pooling(GameObject objectToPool, float time, )
        // {
        //     originRotate = objectToPool.transform.rotation;
        //     // Initialize the object pool
        //     for (int i = 0; i < poolSize; i++)
        //     {
        //         GameObject obj = Instantiate(objectToPool, transform, true);
        //         obj.SetActive(false);
        //         pooledObjects.Add(obj);
        //     }
        //
        //     InvokeRepeating("ActivateObject", 0f, time);
        // }
        //
        // private void ActivateObject()
        // {
        //     int randomNumber = Random.Range(0, 3);
        //     var objectToActive = GetPooledObject();
        //     objectToActive.transform.position = activePosition[randomNumber].position;
        //     objectToActive.transform.rotation = new Quaternion(0, 0, 0, 0);
        //     objectToActive.SetActive(true);
        // }
        //
        // public GameObject GetPooledObject()
        // {
        //     // Find and return an inactive object from the pool
        //     for (int i = 0; i < pooledObjects.Count; i++)
        //     {
        //         if (!pooledObjects[i].activeInHierarchy)
        //         {
        //             return pooledObjects[i];
        //         }
        //     }
        //
        //     // If no inactive objects are found, expand the pool
        //     GameObject obj = Instantiate(objectToPool, transform, true);
        //     obj.SetActive(false);
        //     pooledObjects.Add(obj);
        //     return obj;
        // }
    }
}