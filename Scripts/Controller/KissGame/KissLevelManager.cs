using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unicorn.Unicorn.Scripts.Controller.KissGame.FSM;
using UnityEngine;
using UnityEngine.UI;


namespace Unicorn.Unicorn.Scripts.Controller.KissGame
{
    public class KissLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
        public float delayWin = 0.5f;
        public float heighWin = 1.78f;
        public Image readyToTap;
        public int numberToWin;
        //public Slider slider;
        private int numberSlider;
        
        public TextMeshProUGUI tapText;
        public TextMeshProUGUI timePlay;
        public TextMeshProUGUI txtContain;

        public float timeGamePlay = 30;
        public bool canTap;
        public GameObject characterKiss;
        public List<Rigidbody> listObject;
        

        // private PoolingDam poolingItem;
        // public Transform[] activePosition;
        // public List<GameObject> listOfKiss;
        /*************************************************** game  ******************************************************************/
        
        public new static KissLevelManager Instance => LevelManager.Instance as KissLevelManager;
        KissLobbyAction KisslobbyAction;
        KissInGameAction KissinGameAction;
        KissEndGameAction KissendGameAction;
        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            KisslobbyAction = new KissLobbyAction(GameManager.Instance, lobbyGameState);
            KissinGameAction = new KissInGameAction(GameManager.Instance, inGameState);
            KissendGameAction = new KissEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(KisslobbyAction);
            inGameState.AddAction(KissinGameAction);
            endGameState.AddAction(KissendGameAction);
        }
        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(KisslobbyAction);

            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(KissinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(KissendGameAction);
        }

        private int count = 0;
        public override void StartLevel()
        {
            count++;
            print("count: "+ count);
            numberSlider = numberToWin;
            tapText.enabled = false;
            readyToTap.gameObject.SetActive(false);
            InvokeRepeating("ActiveGravity",3f,3f);
        }

        public void ActiveGravity()
        {
            print("ACTIVE GRAVITY");
            if (listObject.Count==0) return;
            int ranNum = Random.Range(0, listObject.Count);
            listObject[ranNum].useGravity = true;
            int temp =listObject[ranNum].transform.position.x<0?1:2;
            print("TEMP: "+ temp);
            characterKiss.GetComponent<CharacterKiss>().leftOrRight = temp;
            listObject.Remove(listObject[ranNum]);
        }
        
        public void OnUpdateInGame()
        {
            if (GameManager.Instance.CurrentLevel == 1)
            {
                if (canTap)
                {
                    Tut();
                    canTap = false;
                }
                if (readyToTap.IsActive())
                {
                    HideDelay();
                }
            }
            
            if (GameManager.Instance.CurrentLevel == 2)
            {
                tut.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine(TurnOFFTut());
            }

            timeGamePlay -= Time.deltaTime;
            if (timeGamePlay <= 0)
            {
                EndGame(LevelResult.Lose);
            }

        }
        public void Tut()
        {
            Time.timeScale = 0;

            // Hiển thị và nhấp nháy text "TAP" sử dụng DOTween
            tapText.enabled = true;
            tapText.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo); // Nhấp nháy text "TAP"

            // Chờ cho đến khi người chơi nhấp vào màn hình và tăng dần Time.timeScale
            StartCoroutine(ResumeGame());
        }
// || Input.touchCount > 0
        IEnumerator ResumeGame()
        {
            yield return new WaitUntil(() => Input.GetMouseButton(0));

            tapText.DOKill(); // Dừng nhấp nháy
            tapText.enabled = false; // Ẩn text "TAP"
        
            Time.timeScale = 1;
        }

        // public void ChangeSlider()
        // {
        //     slider.value += 1f / numberSlider;
        // }

        public void HideDelay()
        {
            Invoke("Hide", 0.2f);
        }
        void Hide()
        {
            readyToTap.gameObject.SetActive(false);
        }

        
        public override void UpdateUI()
        {
            txtContain.text = "Contain: " + numberToWin;
            if (timeGamePlay >= 10)
            {
                timePlay.text = "00:" + (int)timeGamePlay;
            }
            else
            {
                timePlay.text = "00:0" + (int)timeGamePlay;
            }
        }
        
        public override void Win()
        {
            Invoke("SetCamForAnim", 0.5f);
            base.Win();
        }
        public void SetCamForAnim()
        {
            characterKiss.transform.position = Vector3.zero + Vector3.up;
            characterKiss.transform.eulerAngles = new Vector3(0, 180, 0);
            characterKiss.GetComponent<Animator>().SetBool("Win", true);
        }
     
        public GameObject RandomItemInList(List<GameObject> list)
        {
            int ranNum =Random.Range(0, list.Count);
            return list[ranNum];
        }

        public override void SetDifficulty()
        {

        }

        public void End()
        {
            EndGame(LevelResult.Lose);
        }

        // public void PoolingItem()
        // {
        //     poolingItem = gameObject.AddComponent<PoolingDam>();
        //     poolingItem.objectToPool = RandomItemInList(listOfKiss);
        //     poolingItem.poolSize = 5;
        //     poolingItem.activePosition = activePosition;
        //     poolingItem.Pooling();
        // }

 
       
    }
}