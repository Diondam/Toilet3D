using System.Collections.Generic;
using TMPro;
using Unicorn.Unicorn.Scripts.Controller.CandyGame.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.CandyGame
{
    public class CandyLevelManager : LevelManager
    {
        public int poolSize = 10;
        public float spawnInterval = 1f; // Time interval between spawns
        public float spawnRangeX = 5f; // Range of X-axis positions
        public List<GameObject> candies;
        
        public Transform spawnPoint;

        private List<GameObject> pooledObjects = new List<GameObject>();

        //public float destroyTime = 0.5f;        
        public TextMeshProUGUI txtContain;
        public int numbersNeedToWin = 5;
        public TextMeshProUGUI timePlay;
        public float timeGamePlay = 30;

        public GameObject characterCandy;
            
        //private Quaternion originRotate;
        public new static CandyLevelManager Instance => LevelManager.Instance as CandyLevelManager;
        public bool isWin { get; set; }
        CandyLobbyAction CandylobbyAction;
        CandyInGameAction CandyinGameAction;
        CandyEndGameAction CandyendGameAction;

        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            CandylobbyAction = new CandyLobbyAction(GameManager.Instance, lobbyGameState);
            CandyinGameAction = new CandyInGameAction(GameManager.Instance, inGameState);
            CandyendGameAction = new CandyEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(CandylobbyAction);
            inGameState.AddAction(CandyinGameAction);
            endGameState.AddAction(CandyendGameAction);
        }
        

        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(CandylobbyAction);
            CandylobbyAction.OnExit();

            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(CandyinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(CandyendGameAction);
        }

        public override void StartLevel()
        {
        }

        public override void SetDifficulty()
        {
        }

        public override void UpdateUI()
        {
            txtContain.text = "Contain: " + numbersNeedToWin;
            if (timeGamePlay >= 10)
            {
                timePlay.text = "00:" + (int)timeGamePlay;
            }
            else
            {
                timePlay.text = "00:0" + (int)timeGamePlay;
            }
        }

        public void OnUpdateInGame()
        {
            timeGamePlay -= Time.deltaTime;
            if (timeGamePlay <= 0)
            {
                EndGame(LevelResult.Lose);
            }
        }

        
        public override void Win()
        {
            Invoke("SetCamForAnim", 0.5f);
            base.Win();
        }
        public void SetCamForAnim()
        {
            //characterCandy.transform.position = Vector3.zero + Vector3.up;
            characterCandy.transform.eulerAngles = new Vector3(0, 180, 0);
            characterCandy.GetComponent<Animator>().SetBool("Win", true);
            characterCandy.GetComponent<MoveDirection>().enabled= false;
        }
        public GameObject RandomItemInList(List<GameObject> list)
        {
            int ranNum =Random.Range(0, list.Count);
            return list[ranNum];
        }
        public void Pooling()
        {
            // originRotate = objectToPool.transform.rotation;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(RandomItemInList(candies), transform, true);
                obj.transform.parent = transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }

            InvokeRepeating("SpawnRandomObject", 0f, spawnInterval);
        }

        private void SpawnRandomObject()
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            var objectToActive = GetPooledObject();
            // Y and Z are kept at 0
            objectToActive.transform.position = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z);
            objectToActive.SetActive(true);
        }

        public GameObject GetPooledObject()
        {
            // Find and return an inactive object from the pool
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            // If no inactive objects are found, expand the pool
            GameObject obj = Instantiate(RandomItemInList(candies), transform, true);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
    }
}