using UnityEngine;

namespace Unicorn.Examples
{
    public class Character : MonoBehaviour
    {
        private SkinChangerRoblox skinCharacter;

        public SkinChangerRoblox SkinCharacter
        {
            get => skinCharacter;
            protected set => skinCharacter = value;
        }

        [field: SerializeField]
        public bool IsPlayer { get; set; }

		public bool IsWin{ get; set; }


		protected virtual void Awake()
        {
            skinCharacter = GetComponent<SkinChangerRoblox>();
        }

        protected virtual void Start()
        {
            SkinCharacter.Init(this);
			controller = GetComponent<CharacterController>();
		}


		public float moveSpeed = 5.0f;
		public float jumpForce = 8.0f;
		public float gravity = 20.0f;

		private CharacterController controller;
		private Vector3 moveDirection;

		//các hàm có tính chất khai báo
		//dùng thì sẽ lấy 
		public void UpdateInGame()
		{
			if (controller.isGrounded)
			{
				// Get input from the player
				float horizontalInput = Input.GetAxis("Horizontal");
				float verticalInput = Input.GetAxis("Vertical");

				// Calculate the move direction
				moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
				moveDirection = transform.TransformDirection(moveDirection); // Convert to world space

				// Apply move speed
				moveDirection *= moveSpeed;

				// JumpController
				if (Input.GetButtonDown("JumpController"))
				{
					moveDirection.y = jumpForce;
				}
			}

			// Apply gravity
			moveDirection.y -= gravity * Time.deltaTime;

			// Move the character
			controller.Move(moveDirection * Time.deltaTime);
		}
		//như hàm này có thể call liên tục nhưng thực thi thì chỉ 1 lần do có biến
		//dùng trong update như nào.
	
		public int count { get; set; }

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Box"))
			{
				count++;
				other.gameObject.SetActive(false);
			}
		}
	}
}