using System;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Fly
{
    public class TestD: MonoBehaviour
    {

        private void Start()
        {
               transform.Translate(0, 0, Time.deltaTime);
        }
        private void Update()
        {
          transform.Translate(0, 0, Time.deltaTime, Space.World);
        }
    }
}