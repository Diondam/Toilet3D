using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn.Unicorn.Scripts.Controller.SurfGame.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.SurfGame
{
    public class SurfLevelManager : LevelManager
    {
        /*-------------------------game----------------------------------*/
        public int numbersNeedToWin = 5;
        public float enemySpeed = 16.5f;
        public float mapSpeed = 16.25f;
        public float timeGamePlay = 30;
        public TextMeshProUGUI txtContain;
        public TextMeshProUGUI timePlay;
        public GameObject vfx;
        public GameObject character;

        public List<GameObject> listObjectsToPool;
        public GameObject mapToPool;
        public GameObject Trap;
        private PoolingDam poolTrap;

        public int poolSize = 5;
        public int poolSizeMap = 3;
        private List<GameObject> pooledObjects = new List<GameObject>();
        private List<GameObject> pooledMap = new List<GameObject>();
        public Transform[] activePosition;
        public Transform mapActivePoint;
        public float time = 1.04f;
        private Quaternion originRotate;

        /*-------------------------game----------------------------------*/

        /*-------------------------level----------------------------------*/
        public new static SurfLevelManager Instance => LevelManager.Instance as SurfLevelManager;

        SurfLobbyAction SurflobbyAction;
        SurfInGameAction SurfinGameAction;
        SurfEndGameAction SurfendGameAction;
        /*-------------------------level----------------------------------*/

        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            SurflobbyAction = new SurfLobbyAction(GameManager.Instance, lobbyGameState);
            SurfinGameAction = new SurfInGameAction(GameManager.Instance, inGameState);
            SurfendGameAction = new SurfEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(SurflobbyAction);
            inGameState.AddAction(SurfinGameAction);
            endGameState.AddAction(SurfendGameAction);
        }

        protected override void Start()
        {
            base.Start();
            vfx = Instantiate(vfx, transform.position, Quaternion.identity);
            vfx.transform.parent = transform;
            foreach (var o in listObjectsToPool)     
            {
                print("name: "+ o.name+" Rotation: "+ o.transform.eulerAngles);
            }
            //vfx.SetActive(false);
        }

        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            var preparationState = GameManager.Instance.GameStateController.InGameState;

            inGameState.RemoveAction(SurflobbyAction);
            preparationState.RemoveAction(SurfinGameAction);
            endGameState.RemoveAction(SurfendGameAction);

            SurflobbyAction.OnExit();
        }

        public override void StartLevel() //ingameaction.onenter
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

        public void PoolingTrap()
        {
            poolTrap = gameObject.AddComponent<PoolingDam>();
            poolTrap.objectToPool = Trap;
            poolTrap.poolSize = 5;
            poolTrap.activePosition = activePosition;
            poolTrap.Pooling();
        }

        public GameObject RandomItemInList(List<GameObject> list)
        {
            int ranNum = Random.Range(0, list.Count);
            return list[ranNum];
        }

        public void Pooling()
        {
            //originRotate = objectToPool.transform.rotation;
            // Initialize the object pool
            for (int i = 0; i < 3; i++)
            {
                GameObject prefab = RandomItemInList(listObjectsToPool);
                //print("before rotation: "+ prefab.transform.rotation);
                GameObject obj = Instantiate(prefab, transform.position, prefab.transform.rotation);
                //print("after rotation: "+ obj.transform.rotation);
                obj.transform.parent = transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }

            InvokeRepeating("InvokeActive", 0, 1);
        }

        void InvokeActive()
        {
            float time = Random.Range(0, 0.8f);
            Invoke("ActivateObject", time);
        }

        private void ActivateObject()
        {
            int randomNumber = Random.Range(0, 3);
            var objectToActive = GetPooledObject();
            objectToActive.transform.position = activePosition[randomNumber].position;
            //objectToActive.transform.rotation = originRotate;
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
            GameObject prefab = RandomItemInList(listObjectsToPool);
            GameObject obj = Instantiate(prefab, transform.position, prefab.transform.rotation);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        /*************************************************** Pooling Map ***********************************************************************/


        public void PoolingMap()
        {
            //originRotate = objectToPool.transform.rotation;
            // Initialize the object pool
            for (int i = 0; i < poolSizeMap; i++)
            {
                GameObject obj = Instantiate(mapToPool, transform, true);
                obj.SetActive(false);
                pooledMap.Add(obj);
            }
            //InvokeRepeating("ActivateMap", 0f, existTimeMap);
        }


        public void ActivateMap()
        {
            //get map
            var objectToActive = GetPooledMap();
            //set map position
            objectToActive.transform.position = mapActivePoint.position;
            // objectToActive.transform.rotation = new Quaternion(0, 0, 0, 0);
            objectToActive.SetActive(true);
            print("Đã active map");
        }

        public GameObject GetPooledMap()
        {
            // Find and return an inactive object from the pool
            for (int i = 0; i < pooledMap.Count; i++)
            {
                if (!pooledMap[i].activeInHierarchy)
                {
                    return pooledMap[i];
                }
            }

            // If no inactive objects are found, expand the pool
            GameObject obj = Instantiate(mapToPool, transform, true);
            obj.SetActive(false);
            pooledMap.Add(obj);
            return obj;
        }
    }
}