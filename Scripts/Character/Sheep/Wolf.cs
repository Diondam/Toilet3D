using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Character.Sheep
{
    public class Wolf: MonoBehaviour
    {
        public float speed = 5.0f; // Tốc độ di chuyển của nhân vật
        public float leftLimit = -7.0f; // Giới hạn trái
        public float rightLimit = 7.0f; // Giới hạn phải
        private int direction = 1; // Hướng di chuyển, 1 là phải và -1 là trái

        void Update()
        {
            // Di chuyển nhân vật
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            // Kiểm tra nếu nhân vật đến giới hạn và đổi hướng
            if(transform.position.x >= rightLimit || transform.position.x <= leftLimit)
            {
                direction *= -1; // Đổi hướng
                FlipCharacterDirection();
            }
        }

        void FlipCharacterDirection()
        {
            // Quay nhân vật lại hướng ngược lại
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}