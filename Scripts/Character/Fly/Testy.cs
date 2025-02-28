//
//
// using System;
// using UnityEngine;
//
// namespace Unicorn.Unicorn.Scripts.Character.Fly
// {
//     public class Testy : MonoBehaviour
//     {
//         public float speed = 10.0f; // Tốc độ di chuyển về phía trước của tàu
//         public float turnSpeed = 2.0f; // Tốc độ quay khi di chuyển chuột
//         private Vector3 originalRotation; // Góc quay ban đầu
//         public float limitAngle = 20f;
//
//         private void Start()
//         {
//             // Lưu lại góc quay ban đầu của tàu
//             originalRotation = transform.eulerAngles;
//         }
//
//      
//         void Update()
//         {
//             // Di chuyển tàu về phía trước
//             transform.Translate(Vector3.forward * speed * Time.deltaTime);
//             if (Input.GetMouseButton(0))
//             {
//                 // Lấy thông tin di chuyển chuột
//                 float h = Input.GetAxis("Mouse X") * turnSpeed;
//                 print("h: "+ h);
//                 float v = Input.GetAxis("Mouse Y") * turnSpeed;
//                 print("v: "+ v);
//                 // Tính góc quay mới và giới hạn nó để không quá 90 độ so với góc ban đầu
//                 float newPitch = Mathf.Clamp(transform.eulerAngles.x - v, originalRotation.x - limitAngle,
//                     originalRotation.x + limitAngle);
//                 float newYaw = Mathf.Clamp(transform.eulerAngles.y + h, originalRotation.y - limitAngle,
//                     originalRotation.y + limitAngle);
//
//                 // Sử dụng Euler để cập nhật góc quay mới với giới hạn
//                 transform.eulerAngles = new Vector3(newPitch, newYaw, originalRotation.z);
//             }
//         }
//     }
// }


using System;
using Unicorn.Unicorn.Scripts.Controller.FlyGame;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Fly
{
    public class Testy : MonoBehaviour
    {
        public float speed = 10.0f; // Tốc độ di chuyển về phía trước của tàu
        public float turnSpeed = 2.0f; // Tốc độ quay khi di chuyển chuột
        private Vector3 originalRotation; // Góc quay ban đầu
        private Vector3 currentRotation; // Góc quay hiện tại để giới hạn
        public float angle = 30f;

        private void Start()
        {
            originalRotation = transform.eulerAngles;
            currentRotation = transform.eulerAngles;
        }

        private void OnTriggerEnter(Collider other)
        {
            print("WIN");
            if (other.gameObject.CompareTag("WinPlace"))
            {
                
                FlyLevelManager.Instance.Win();
            }
        }

        void Update()
        {
            // Di chuyển tàu về phía trước
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Input.GetMouseButton(0))
            {
                // Lấy thông tin di chuyển chuột
                float h = Input.GetAxis("Mouse X") * turnSpeed;
                float v = Input.GetAxis("Mouse Y") * turnSpeed;

                // Cập nhật góc quay hiện tại và giới hạn nó
                currentRotation.x += -v;
                currentRotation.y += h;
                currentRotation.x = Mathf.Clamp(currentRotation.x, originalRotation.x - angle,
                    originalRotation.x + angle);
                currentRotation.y = Mathf.Clamp(currentRotation.y, originalRotation.y - angle,
                    originalRotation.y + angle);

                // Áp dụng góc quay mới với giới hạn
                transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, originalRotation.z);
            }
        }
    }
}