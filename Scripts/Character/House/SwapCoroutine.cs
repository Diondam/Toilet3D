using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class SwapCoroutine : MonoBehaviour
{
    public Transform[] objectsToSwap;
    public Transform mooring1;
    public Transform mooring2;
    public float swapInterval = 3.0f; // Time interval for swapping
    public int loop = 3;
    private bool swapped = false;
    private bool isEndSwap = false;
    public GameObject pumpkin;
    private void Start()
    {
        StartCoroutine(SetUp());
    }

    private void Update()
    {
        if (isEndSwap)
        {
            //print("CHECK TOUCH");
            if (Input.GetMouseButtonDown(0)) // Kiểm tra xem có bao nhiêu ngón tay đang chạm vào màn hình
            {
                print("Đã chạm màn hình");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    print("Đã chạm :" + hit.transform.gameObject.name);
                    // Nếu dò ray chạm vào đối tượng này
                    if (hit.transform.gameObject.tag == "House")
                    {
                        print("QUAY");
                        StartCoroutine(RotateBack(hit.transform));
                        if (pumpkin.transform.parent == hit.transform)
                        {
                            Debug.Log("GameObject B là con của GameObject A");
                        }
                    }
                }
            }
        }
    }


    // IEnumerator Move()
    // {
    //     int index1 = Random.Range(0, objectsToSwap.Length);
    //     int index2 = (index1 + 1) % objectsToSwap.Length;
    //     int index3 = (index2 + 1) % objectsToSwap.Length;
    //     StartCoroutine(Swap(objectsToSwap[index1], objectsToSwap[index2], swapInterval));
    //     yield return new WaitForSeconds(swapInterval);
    //     StartCoroutine(Swap(objectsToSwap[index1], objectsToSwap[index3], swapInterval));
    //     yield return new WaitForSeconds(swapInterval);
    //     StartCoroutine(Swap(objectsToSwap[index1], objectsToSwap[index3], swapInterval));
    //     yield return new WaitForSeconds(swapInterval);
    // }

    
    IEnumerator SetUp()
    {
        int randomIndex = Random.Range(0, objectsToSwap.Length);
        pumpkin.transform.SetParent(objectsToSwap[randomIndex], true);
        pumpkin.transform.localPosition= new Vector3(0,0,0);
        yield return new WaitForSeconds(0f);
        // StartCoroutine(RotateBack(objectsToSwap[0]));
        // StartCoroutine(RotateBack(objectsToSwap[1]));
        // StartCoroutine(RotateBack(objectsToSwap[2]));
        // yield return new WaitForSeconds(1f);
        rotationProgress = 0;
        StartCoroutine("SwapPhrase");
    }
    

    public float rotationProgress = 0;
    public float rotationSpeed = 1.0f;
    IEnumerator RotateBack(Transform objectToBack)
    {
        
        var initialRotation = objectToBack.rotation;
        var targetRotation = Quaternion.Euler(objectToBack.eulerAngles + new Vector3(0, 180, 0));
        while (rotationProgress <= 1)
        {
            print("ROTATE");
            rotationProgress += Time.deltaTime * rotationSpeed;
            objectToBack.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);

            if (rotationProgress >= 1)
            {
                objectToBack.rotation = targetRotation; // Đảm bảo đối tượng đã quay đúng 180 độ
            }
            yield return null;
        }
    }

    IEnumerator SwapPhrase()
    {
        for (int i = 0; i < loop; i++)
        {
            int index1 = Random.Range(0, objectsToSwap.Length);
            int index2 = (index1 + 1) % objectsToSwap.Length;
            StartCoroutine(Swap(objectsToSwap[index1], objectsToSwap[index2], swapInterval));
            yield return new WaitForSeconds(swapInterval);
        }

        isEndSwap = true;
    }

    IEnumerator Swap(Transform a, Transform b, float duration)
    {
        float elapsedTime = 0.0f;
        Vector3 aPos = a.position;
        Vector3 bPos = b.position;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            a.position = Quadratic(aPos, mooring1.position, bPos, t);
            b.position = Quadratic(bPos, mooring2.position, aPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        a.position = bPos;
        b.position = aPos;
    }

    public Vector3 Quadratic(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
}