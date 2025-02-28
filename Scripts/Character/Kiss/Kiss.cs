using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Kiss : MonoBehaviour
{
    public float kissSpeed=1f;
    private float t;
    private bool trigger = false;
    public Vector3 pointKiss;
    private void Update()
    {
        if (t < 1)
        {
            t += Time.deltaTime * kissSpeed;
        }
        else
        {
            trigger = true;
        }
        if(Input.GetMouseButtonUp(0)){
            trigger = true;
        }
        if (trigger)
        {
           // Vector3 targetPosition = Vector3.Lerp(transform.position,new Vector3(0,1,0),t);
            Vector3 targetPosition = Vector3.Lerp(transform.position,pointKiss,t);

            transform.position = targetPosition;
        }
    }
    
}