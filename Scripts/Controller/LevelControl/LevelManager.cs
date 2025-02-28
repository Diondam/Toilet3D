using System;
using System.Collections;
using Unicorn.Examples;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Unicorn
{
    /// <summary>
    /// Quản lý riêng từng level.
    /// </summary>
    /// <remarks>
    /// Inherit class này rồi đặt vào object của scene Level
    /// </remarks>
    public abstract class LevelManager : MonoBehaviour
    {
        private static LevelManager instance;
        public static LevelManager Instance => instance;

        public new Camera camera;

        //Mỗi 1 level sẽ có 1 Result nó sẽ được truy cập để sử dụng trong Endgame
        public LevelResult Result { get; set; }
        public Canvas tut;
        public Button PlayGamePlay;
        
        
        public float timeDelayWin = 1f;

        //những hàm này vẫn sẽ được Unity gọi mà không cần dùng class con ghi đè lại rồi dùng instance để gọi
        protected virtual void Awake()
        {
            instance = this;
            //SetDifficulty();
            print("LEVELMANAGER+ " + Instance + " Awake!");
            PlayGamePlay?.onClick.AddListener(ChangeStateToInGame);
            PlayGamePlay?.gameObject.SetActive(false);
        }

        private void ChangeStateToInGame()
        {
            GameManager.Instance.StartLevel();
            PlayGamePlay.gameObject.SetActive(false);
        }
        protected virtual void Start()
        {
            //print("LEVELMANAGER+ " + Instance + " Start!");
            //đưa result của level về no Decided(chưa quyết định)
            SetUpLevelEnvironment();
            GameManager.Instance.RegisterLevelManager(this);
        }

        /// <summary>
        /// Khởi tạo để nối hệ thống chung với hệ thống riêng của mỗi level. 
        /// </summary>
        protected virtual void SetUpLevelEnvironment()
        {
            ResetLevelState();
        }

        /// <summary>
        /// Làm mới level
        /// </summary>
        public virtual void ResetLevelState()
        {
            Result = LevelResult.NotDecided;
        }

        /// <summary>
        /// set các animaniton, quay cam ở đây, anim coroutine các đối tượng game
        /// </summary>
        public virtual void StoryOnLobby()
        {
            //chạy anim onstart
            StartCoroutine("AnimOnStart");
            //display tap to play

            //hide tap to play

            //change state to Ingame
        }

        public virtual IEnumerator AnimOnStart()
        {
            //anim 1

            //quay cam

            //anim 2

            //quay cam

            yield return new WaitForSeconds(1f);
        }
        //nếu mà cần thực hiện các action cần lặp qua từng frame thì dùng trong Update
        //nếu nó là các croutine: tinh chỉnh theo từng đoạn frame thì Start ra luồng khác

        //trên đây thực hiện 1 số việc và lớp con sẽ thực hiện thêm
        public virtual void IncreaseCurrentLevel()
        {
            if (Result == LevelResult.Win)
            {
                //tăng độ khó rồi reload lại scene?, thế data đã tăng trong level này sẽ mất thì sao truyền vào level sau
                //có thể là 1 cơ chế phức tạp hơn cho game hệ thống lớn hơn, lưu data sang PlayPref chẳng hạn xong lấy từ đây ra để tăng
                //thay đổi nhiều data, còn con game simple này thì chỉ cần tăng trực tiếp theo level hiện tại
                //IncreaseDifficulty();
            }
            else if (Result == LevelResult.Lose)
            {
                //có thể có cái gì đó vào đây
            }

            GameManager.Instance.ReloadLevel();
        }

        //lấy level hiện tại vd 1,2,3 từ đó tăng chỉ số hoặc gì đó khác ở đây
        //lớp con sẽ có kiểu khó riêng
        /// <summary>
        /// Có thể đặt tên là Quy định độ khó cho game Difficulty
        /// </summary>
        public virtual void SetDifficulty()
        {
            //somthing default here
        }

        /// <summary>
        /// WIN có delay/
        /// sẽ có mặc định delay win cho các level, các level con có thể có thêm thời gian delay cho animation
        /// </summary>
        public virtual void Win()
        {
            Invoke("EndGameWin", timeDelayWin);
        }

        void EndGameWin()
        {
            EndGame(LevelResult.Win);
            SetUpCamera();
        }

        private void SetUpCamera()
        {
            print("Setup VM cam");
            //chỉnh VM cam thấp hơn
            camera.depth = -2;
            CameraController mainCamera = GameManager.Instance.MainCamera;
            //đưa cam UI đến chỗ cam VM
            mainCamera.transform.position = camera.transform.position;
            mainCamera.transform.rotation = camera.transform.rotation;
            mainCamera.Camera.fieldOfView = camera.fieldOfView;
        }


        /// <summary>
        /// Khởi tạo những thứ cần thiết và bắt đầu level
        /// </summary>
        public abstract void StartLevel();

        /// <summary>
        /// Nên gọi hàm này để kết thúc level, tránh gọi EndGame ở GameManager, vì có thể có nhiều gameplay
        /// </summary>
        /// <param name="levelResult"></param>
        protected virtual void EndGame(LevelResult levelResult)
        {
            //nếu kết quả hiện tại không không xác định: win or lose rồi thì thôi = đã xác định rồi
            //hàm này mục đích là để set kết quả level vào biến Result
            if (Result != LevelResult.NotDecided)
            {
                Debug.LogWarning(
                    $"Level has already ended with result ${Result} but another request for result ${levelResult} is still being sent!");
                return;
            }

            //nếu Result đang ở không xác định(vừa chơi xong thì nó vẫn chưa đổi trạng thái)
            //thì gán biến win hoặc lose vào đây
            Result = levelResult;
            //change state sang endgame và tăng level và lưu vào datalevel
            GameManager.Instance.DelayedEndgame(levelResult);
        }


        public virtual void UpdateUI()
        {
        }

        public void DisplayTut()
        {
            int level = GameManager.Instance.CurrentLevel;
            if (level == 1)
            {
                //tut.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine(TurnOFFTut());
            }
            else
            {
                tut.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        public IEnumerator TurnOFFTut()
        {
            yield return new WaitForSeconds(3f);
            tut.transform.GetChild(0).gameObject.SetActive(false);
        }

        // private void Update()
        // {
        //     print("Instance: "+ Instance);
        // }

        protected virtual void OnDestroy()
        {
        }
    }
}