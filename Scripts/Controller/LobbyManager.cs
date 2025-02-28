using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn.Examples
{

    //thực ra nên đặt tên lại là Level1Manager
    public class LobbyManager : LevelManager
    {

		
        [SerializeField] private new Camera camera;

        protected override void Awake()
        {	      

            base.Awake();
            SetUpCamera();
        }

      
        public override void StartLevel()
        {
	        //DÒNG NÀY VÔ NGHĨA BỞI VÌ NÓ CHƯA HỀ CHUYỂN SANG iNGAME ĐÃ END LELVEL RỒI
	        GameManager.Instance.DelayedEndgame(LevelResult.Win);
			
        }


		private void SetUpCamera()
		{
			CameraController mainCamera = GameManager.Instance.MainCamera;
			var mainCameraTransform = mainCamera.transform;
			mainCameraTransform.position = camera.transform.position;
			mainCameraTransform.rotation = camera.transform.rotation;
			mainCamera.Camera.fieldOfView = camera.fieldOfView;
		}
		
		

	}

}