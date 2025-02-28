using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeMove : MonoBehaviour
{
    private Vector3 touchStartPos;
    private Vector3 objectStartPos;
    private bool isDragging = false;

    public float heighDecrease = 10;
    public float heighIncrease = 0.1f;
    public float minHeigh = -3f;

    public float moveSpeed = 5.0f;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    bool isIncreaseY = true;
    public float maxHeigh = 4;

    IEnumerator Increase()
    {
        isIncreaseY = true;
        float time = 0;
        while (time < 0.5)
        {
            time += Time.deltaTime;
            float tempY2 = transform.position.y;
            if (transform.position.y >= maxHeigh)
            {
                isIncreaseY = false;
                yield break;
            }

            tempY2 += heighIncrease * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, tempY2, transform.position.z);
            yield return null;
        }

        isIncreaseY = false;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //increase
            if (transform.position.y <= maxHeigh)
                StartCoroutine(Increase());
            if (Input.mousePosition.x < Screen.width / 2)
            {
                isMovingLeft = true;
                isMovingRight = false;
            }
            else
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2)
            {
                isMovingLeft = true;
                isMovingRight = false;
            }
            else
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
        }

        //decrease
        if (!isIncreaseY)
        {
            float tempY = transform.position.y;
            if (tempY > minHeigh)
                tempY -= Time.deltaTime * heighDecrease;
            transform.position = new Vector3(transform.position.x, tempY, transform.position.z);
        }


        if (isMovingLeft && transform.position.x > -4)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (isMovingRight && transform.position.x < 4)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }
}