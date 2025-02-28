using System;
using System.Collections;
using UnityEngine;

public class ObjectSwapper : MonoBehaviour
{
    public Transform[] ListOfSwap;
    public float interpolateAmount;
    private Vector3 house1;
    private Vector3 house2;
    private Vector3 house3;

    private void Start()
    {
        UpdatePos();
    }

    void UpdatePos()
    {
        house1 = new Vector3(ListOfSwap[0].position.x, ListOfSwap[0].position.y, ListOfSwap[0].position.z);
        house2 = new Vector3(ListOfSwap[2].position.x, ListOfSwap[2].position.y, ListOfSwap[2].position.z);
        house3 = new Vector3(ListOfSwap[4].position.x, ListOfSwap[4].position.y, ListOfSwap[4].position.z);
    }

    private bool temp1 = true;
    private bool temp2 = true;
    private bool temp3 = true;
    private int forr = 0;
    private int frame = 0;
    private void Update()
    {
        print("Frame: "+ frame++);
        interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        for (int i = 0; i < 3; i++)
        {
            print("For:" + forr++);
            SimpleMove();
        }
    }


    void SimpleMove()
    {
        if (temp1)
        {
            ListOfSwap[0].position = Quadratic(house1, ListOfSwap[1].position, house2, interpolateAmount);
            ListOfSwap[2].position = Quadratic(house2, ListOfSwap[3].position, house1, interpolateAmount);
            if (interpolateAmount > 0.994f)
            {
                temp1 = false;
                ListOfSwap[0].position = house2;
                ListOfSwap[2].position = house1;
                UpdatePos();
                interpolateAmount = 0;
            }
        }

        if (!temp1 && temp2)
        {
            ListOfSwap[0].position = Quadratic(house1, ListOfSwap[1].position, house3, interpolateAmount);
            ListOfSwap[4].position = Quadratic(house3, ListOfSwap[3].position, house1, interpolateAmount);
            if (interpolateAmount > 0.994f)
            {
                temp2 = false;
                ListOfSwap[0].position = house3;
                ListOfSwap[4].position = house1;
                UpdatePos();
                interpolateAmount = 0;
            }
        }

        if (!temp2 && temp3)
        {
            ListOfSwap[2].position = Quadratic(house2, ListOfSwap[1].position, house3, interpolateAmount);
            ListOfSwap[4].position = Quadratic(house3, ListOfSwap[3].position, house2, interpolateAmount);
            if (interpolateAmount > 0.994f)
            {
                temp3 = false;
                ListOfSwap[2].position = house3;
                ListOfSwap[4].position = house2;
                UpdatePos();
                interpolateAmount = 0;
            }
        }
    }

    public Vector3 Quadratic(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

   
}