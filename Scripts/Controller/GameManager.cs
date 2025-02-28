using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unicorn.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/***********************************************************


        ╭╮ ╭╮╭━╮ ╭╮╭━━╮╭━━━╮╭━━━╮╭━━━╮╭━╮ ╭╮
        ┃┃ ┃┃┃┃╰╮┃┃╰┫┣╯┃╭━╮┃┃╭━╮┃┃╭━╮┃┃┃╰╮┃┃
        ┃┃ ┃┃┃╭╮╰╯┃ ┃┃ ┃┃ ╰╯┃┃ ┃┃┃╰━╯┃┃╭╮╰╯┃
        ┃┃ ┃┃┃┃╰╮┃┃ ┃┃ ┃┃ ╭╮┃┃ ┃┃┃╭╮╭╯┃┃╰╮┃┃
        ┃╰━╯┃┃┃ ┃┃┃╭┫┣╮┃╰━╯┃┃╰━╯┃┃┃┃╰╮┃┃ ┃┃┃
        ╰━━━╯╰╯ ╰━╯╰━━╯╰━━━╯╰━━━╯╰╯╰━╯╰╯ ╰━╯
        ╭━━━━┳━━━╮╭━━━━┳╮ ╭┳━━━╮╭━━━━┳━━━┳━━━╮
        ┃╭╮╭╮┃╭━╮┃┃╭╮╭╮┃┃ ┃┃╭━━╯┃╭╮╭╮┃╭━╮┃╭━╮┃
        ╰╯┃┃╰┫┃ ┃┃╰╯┃┃╰┫╰━╯┃╰━━╮╰╯┃┃╰┫┃ ┃┃╰━╯┃
          ┃┃ ┃┃ ┃┃  ┃┃ ┃╭━╮┃╭━━╯  ┃┃ ┃┃ ┃┃╭━━╯
          ┃┃ ┃╰━╯┃  ┃┃ ┃┃ ┃┃╰━━╮  ┃┃ ┃╰━╯┃┃
          ╰╯ ╰━━━╯  ╰╯ ╰╯ ╰┻━━━╯  ╰╯ ╰━━━┻╯
                                            
                                                            
                                         .^^                
                                       ^?J~                 
                      :J5YJ?7~~!7?7.:7Y57.                  
                 .^7JY55P5YJ?J5GBP?YP5?:                    
             .~?5PPYJ?777!!77??7?YBBY~                      
          .!YGBGY?!!777!!!777777~??Y57                      
         ^?JP#GY?7777!!^~7777777~77?JPJ.                    
          :YGY7!777!^J5:!777777777???7JY!                   
         7BPYJ7777~:5#B~:~!!!!77777777!7YY!.                
        ?#GGG7777!.J#B#BY77777777!!!!777!7YP7.              
       !#BG#?!777:^B#BB###?!!!!!!!!7?7!7777YY5              
      .GP:YG7777!:~BBBBB#G          .!?!!!77YP              
      ^5. 5P7J77!:^B#BBB#B.           ^^~77!~^              
      ..  YGYG777^.J#BBBB#?                                 
          ~BG#Y!7!::5##BBB#?                   :.           
           Y###Y77!^:JB##B##P~               ^!:            
           .PPJBG?77~:~YB#####PJ~:.     .:^7J?.             
            .? :YGPJ7!~^^75GB####BGPP555PP5?:               
                 :75P5Y?7!~~!7?JY55555YY?~.                 
                    .~7JY5YYYJJJ????7!^.                    
                         ..:^^^^::.                                 

 ***********************************************************/

namespace Unicorn
{
    /// <summary>
    /// Quản lý load scene và game state
    /// </summary>
    public partial class GameManager : SerializedMonoBehaviour
    {

        public static GameManager Instance;

        [Space]
        [BoxGroup("Level")]
        [SerializeField] private LevelConstraint levelConstraint;


        [FoldoutGroup("Persistant Component", true)]
        [SerializeField] private UiController uiController;
        [FoldoutGroup("Persistant Component")]
        [SerializeField] private CameraController mainCamera;

        [FoldoutGroup("Persistant Component")]
        [SerializeField] private IapController iap;

        private LevelManager currentLevelManager;

        private IDataLevel dataLevel;

        public event Action GamePaused;
        public event Action GameResumed;

        public bool IsLevelLoading { get; private set; }

        public ILevelInfo DataLevel => dataLevel;
        public int CurrentLevel => DataLevel.GetCurrentLevel();
        public GameFSM GameStateController { get; private set; }
        public PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;
        public CameraController MainCamera => mainCamera;
        public UiController UiController => uiController;
        public LevelManager LevelManager
        {
            get => currentLevelManager;
            private set => currentLevelManager = value;
        }
        public IapController IapController => iap;
        public Profile Profile { get; private set; }

        private void Awake()
        {
            //đối tượng được gắn scrip này chạy tức là cứ ném vào 1 con đối tượng để nó chạy là được
            Instance = this; // con này dùng để quản lí scene 
            GameStateController = new GameFSM(this); // con này để chuyển state
            Profile = new Profile();

            DOTween.Init().SetCapacity(200, 125);
#if FINAL_BUILD
            Debug.unityLogger.logEnabled = false;
#endif
            //con biến levelConstraint này là thừa
            //con dataLevel này ở lúc ban đầu chắc chắn là chứa các giá trị mặc định của các biến chứa trong nó
            //vd type = Duck, display level = 1, info = các giá trị mặc định của đối tượng LevelTypeInfo
            // dataLevel = PlayerDataManager.GetDataLevel(levelConstraint);
            // dataLevel.LevelConstraint = levelConstraint;
            //sau khi xem sét lại thì trong 2 dòng code trên ,levelConstraint thực sự vô dụng
            //maybe có thể có tác dụng ở phần sau
        }

        private void Start()
        {
            print("CurrentLevel: "+ CurrentLevel);
            UiController.Init();
            LoadLobby();
        }

        /// <summary>
        ///
        /// Load map lobby lên thôi, không cần chơi gì cả vì ấn loadlevel là nó đã load Scene khác rồi
        /// </summary>
        public void LoadLobby()
        {
            //print("*LOAD LOBBY*");
            //nếu mà ến level EndSeason rồi thì load thẳng luôn k cần sang lobby nữa
            // if (dataLevel.LevelType == LevelType.EndSeason)
            // {
            //     LoadLevel();
            //     return;
            // }
            //nếu không thì unload splash, hoặc level cũ
            //check nếu nó có 2 scene thì mới chắc cú có 1 scene master và 1 scene level
            //thực ra nếu cần quay về lobby sau mỗi lần thì mới cần
            // if (SceneManager.sceneCount != 1)
            // {
            //     SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            // }

            // Khi unload scene cũ thì LevelManager cũ cũng bị destroy
            // nhưng cứ gán null cho chắc
            //LevelManager = null;
            IsLevelLoading = true;
            uiController.OpenLoading(true);
            //load lobby
            const int lobbySceneBuildIndex = 2;
            SceneManager.LoadSceneAsync(lobbySceneBuildIndex, LoadSceneMode.Additive);
            UiController.UiMainLobby.Show(true);
            //GameStateController.ChangeState(GameState.LOBBY);
        }

        /// <summary>
        /// Load level mới và xóa level hiện tại
        /// </summary>
        public void LoadLevel()
        {
            mainCamera.AudioListener.enabled = false;
            //get index sau khi đã endlevel trước để chạy game,
            //trong này đã có process tăng level 
            int buildIndex = dataLevel.GetBuildIndex();
#if UNITY_EDITOR
            //dễ dàng tùy chỉnh level cần load và tinh chỉnh trong editor
            buildIndex = GetForcedBuildIndex(buildIndex);
#endif
            //check valid index level, chỉ có thể có các level 1,2,3,4,..
            //nó có lớn hơn index hiện tại(là 1 vì 1 cái là splash 1 cái là master)
            //hoặc là nhỏ hơn tổng index không, nếu nằm trong thì ok
            bool isBuildIndexValid = buildIndex > gameObject.scene.buildIndex && buildIndex < SceneManager.sceneCountInBuildSettings;
            //nếu không nằm trong vùng đấy thì không load, tức vẫn ở lobby
            if (!isBuildIndexValid)
            {
                Debug.LogError("No valid scene is found! \nFailed build index: " + buildIndex);
                GameStateController.ChangeState(GameState.LOBBY);
                return;
            }
            //nếu đúng thì cho loading
            IsLevelLoading = true; // dùng để ràng buộc các việc cùng diễn ra, k chạy Update,FixedUpdate
            //unload LEVEL cũ, nếu SceneManager.sceneCount == 1, tức là có mỗi Master thì load scene mới
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
              
            }
            //load level theo buildindex thêm vào, loadlobyy 
            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            print("LOAD SCENE COMPLETE AT BuildIndex: "+ buildIndex);
            //uiController.OpenLoading(true);
        }

        public void LoadLevelForLobby()
        {
            //lấy từ datalevel ra level vừa mới chơi cũ
            int buildIndex = levelConstraint.GetStartIndex(dataLevel.LevelType);
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            }

            //load level theo buildindex thêm vào, loadlobyy 
            SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
            print("LOAD LEVEL BY LOBBY, buildIndex  "+ buildIndex);
        }
        
        
        public void ChangeLevelType()
        {
            var dataLevel = this.dataLevel as UnicornDataLevel;
            dataLevel.IncreaseLevel();
        }
        
        public void ReloadLevelX()
        {
            //lấy về index của scene gameplay hiệnt tại
            int currentSceneIndex = levelConstraint.GetStartIndex(dataLevel.LevelType);
            if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
                print("ĐÃ UNLOAD SCENE");
            }
            print("BUILD INDEX:"+ currentSceneIndex);
            SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Additive);
        } 
        
        public void Replay()
        {
            // var dataLevel = this.dataLevel as UnicornDataLevel;
            // dataLevel.IncreaseLevelNotChangeLevelType();
            // LevelManager.ResetLevelState();
            // LevelManager.IncreaseCurrentLevel();//= reload level
            ReloadLevel();
        }
        public void ReloadLevel()
        {
            //lấy về index của scene hiệnt tại
            //dựa vào datalevel, get index
            IsLevelLoading = true;
            Scene currentScene = SceneManager.GetSceneAt(1);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            SceneManager.LoadSceneAsync(currentScene.buildIndex, LoadSceneMode.Additive);
        }


        /// <summary>
        /// Đưa game về state Lobby và khởi tạo lại các giá trị cần thiết cho mỗi level mới.
        /// <remarks>
        /// LevelManager khi đã load xong thì PHẢI gọi hàm này.
        /// </remarks>
        /// </summary>
        /// <param name="levelManager"></param>
        /// 
        // mỗi 1 con level mới sẽ gọi con này để load hết các action từ các state vào FSM để chạy
        private int count = 0;
        public void RegisterLevelManager(LevelManager levelManager)
        {
            //Debug.Log("RegisterLevelManager : "+ levelManager); 
            LevelManager = levelManager;
            GameStateController.ChangeState(GameState.LOBBY);
            //các action trong state luôn được FSM gọi, change là để nạp các action vào FSM để gọi
            //FSM được GameManager gọi
            IsLevelLoading = false;
            if (uiController == null)
            {
                print("UIControler is Null");
                return;
            }
            uiController.OpenLoading(IsLevelLoading);
        }

        /// <summary>
        /// Bắt đầu level, đưa game vào state <see cref="GameState.IN_GAME"/>
        /// </summary>
        public void StartLevel()
        {
            Analytics.LogTapToPlay();
            GameStateController.ChangeState(GameState.IN_GAME);
        }

        /// <summary>
        /// Kết thúc game sau một khoảng thời gian
        /// </summary>
        /// <param name="result"></param>
        /// <param name="delayTime"></param>
        public void DelayedEndgame(LevelResult result, float delayTime = 0.5f)
        {
            StartCoroutine(DelayedEndgameCoroutine(result, delayTime));
        }

        private IEnumerator DelayedEndgameCoroutine(LevelResult result, float delayTime)
        {
            yield return Yielders.Get(delayTime);
            EndLevel(result);
        }
        
        

        /// <summary>
        /// Kết thúc gameplay
        /// </summary>
        /// <param name="result"></param>
        public void EndLevel(LevelResult result)
        {
            //không xác định vẫn endgame
            GameStateController.ChangeState(GameState.END_GAME);
            
            if (result == LevelResult.Win)
            {
                var dataLevel = this.dataLevel as UnicornDataLevel;
                //dataLevel.IncreaseLevelNotChangeLevelType();
            }
        }

        
        /// <summary>
        /// Hồi sinh
        /// </summary>
        public void Revive()
        {
            LevelManager.ResetLevelState();
            // TODO: Revive code
        }

        public void Pause()
        {
            Time.timeScale = 0;
            GamePaused?.Invoke();
        }

        public void Resume()
        {
            Time.timeScale = 1;
            GameResumed?.Invoke();
        }

        private void Update()
        {
          //  print("LevelTypee"+ LevelManager);
            if (!IsLevelLoading)
                GameStateController?.Update();
        }

        private void FixedUpdate()
        {
            if (!IsLevelLoading)
                GameStateController?.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (!IsLevelLoading)
                GameStateController?.LateUpdate();
        }
    }
}

