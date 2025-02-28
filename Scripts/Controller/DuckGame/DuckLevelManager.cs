using System.Collections.Generic;
using TMPro;
using Unicorn.Unicorn.Scripts.Controller.DuckGame.FSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Unicorn.Unicorn.Scripts.Controller.DuckGame
{
    public class DuckLevelManager : LevelManager
    {
        public CharacterPlayer characterDuck;
        public float intendDifficulty = 10;
        public float duckSpeed = 5;
        public float spawnRadius = 10.0f;
        public float timeGamePlay = 30;
        public float minDistanceToPlayer = 3.0f;
        public TextMeshProUGUI txtContain;
        public TextMeshProUGUI timePlay;
        public int numberNeedToWin;
        public GameObject meshPlane;
        public float magnitude=0;
        
        
        #region Level 1,2,3

        public GameObject enemyPrefab;
        public int numberOfEnemies = 5;

        #endregion

        #region Level4

        public GameObject Trap;
        public int numberOfTrap;
        public GameObject uselessItem;
        public int numberOfUselessItem = 10;
        private Vector3[] vertices;
        private Vector3[] normals;

        #endregion

        #region Level 5

        public List<GameObject> speedOrSwordItem;
        public int SOSnumberitem;
        public GameObject HPItem;
        public int numberHPItem;

        public GameObject HPGhost1;
        public int numberHPGhost1;

        #endregion

        #region Level6,7

        public GameObject HPGhost2;
        public int numberHPGhost2;

        #endregion

        #region Level8

        public GameObject HPGhost3;
        public int numberHPGhost3;

        #endregion

        #region Level 9,10

        public GameObject BoomDuck;
        public int numberBoomDuck;

        #endregion


        public new static DuckLevelManager Instance => LevelManager.Instance as DuckLevelManager;
        DuckLobbyAction DucklobbyAction;
        DuckInGameAction DuckinGameAction;
        DuckEndGameAction DuckendGameAction;

        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            DucklobbyAction = new DuckLobbyAction(GameManager.Instance, lobbyGameState);
            DuckinGameAction = new DuckInGameAction(GameManager.Instance, inGameState);
            DuckendGameAction = new DuckEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(DucklobbyAction);
            inGameState.AddAction(DuckinGameAction);
            endGameState.AddAction(DuckendGameAction);
            
        }

        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var preparationState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            inGameState.RemoveAction(DucklobbyAction);
            preparationState.RemoveAction(DuckinGameAction);
            endGameState.RemoveAction(DuckendGameAction);
        }

        protected override void Start()
        {
            base.Start();
           
        }

        public GameObject RandomItemInList(List<GameObject> list)
        {
            int ranNum =Random.Range(0, list.Count);
            return list[ranNum];
        }
        
        public void RandomStuffMesh(int number, GameObject obj)
        {
            var mesh = meshPlane.GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;
            normals = mesh.normals;

            for (int i = 0; i < number; i++)
            {
                Spawn1RandomStuff(obj);
            }
        }

        void Spawn1RandomStuff(GameObject ranObj)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 randomVertex = vertices[randomIndex];
            Vector3 vertexNormal = normals[randomIndex];

            // Kiểm tra xem vector pháp tuyến có hướng lên không
            if (vertexNormal.y > 0.5f) // Số này đảm bảo rằng chỉ chọn những vector pháp tuyến gần với (0,1,0)
            {
                Vector3 worldPos = transform.TransformPoint(randomVertex);
                var obj = Instantiate(ranObj, worldPos, Quaternion.identity);
                obj.transform.parent = transform;
            }
            else
            {
                // Nếu đỉnh được chọn không thích hợp, chọn một đỉnh khác
                Spawn1RandomStuff(ranObj);
            }
        }

        //cái này sẽ được gọi theo vòng lặp
        public override void SetDifficulty()
        {
            // lấy level hiện tại vd 1,2,3 từ đó tăng chỉ số hoặc gì đó khác ở đây
            // duckSpeed += UnicornDataLevel.Curre;
            print("Đã Tăng độ khó!");
            int level = GameManager.Instance.CurrentLevel;
            duckSpeed = level * duckSpeed * intendDifficulty;
            minDistanceToPlayer = level * 1.5f + minDistanceToPlayer;
            //print("level * duckSpeed:  " + level + "*" + duckSpeed);
        }

        public override void UpdateUI()
        {
            txtContain.text = "Contain: " + numberNeedToWin;
            if (timeGamePlay >= 10)
            {
                timePlay.text = "00:" + (int)timeGamePlay;
            }
            else
            {
                timePlay.text = "00:0" + (int)timeGamePlay;
            }
        }

        //logic game play here
        public void OnUpdateInGame()
        {
            timeGamePlay -= Time.deltaTime;
            if (timeGamePlay <= 0)
            {
                EndGame(LevelResult.Lose);
            }
        }

        
        public void Lose()
        {
            EndGame(LevelResult.Lose);
        }
        public override void Win()
        {
            characterDuck.GetComponent<CharacterController>().enabled = false;
            Invoke("SetCamForAnim", 0.5f);
            //InvokeRepeating("SetCamForAnim", 0.1f, 0.1f);
            base.Win();
        }
        public void SetCamForAnim()
        {
            characterDuck.transform.position = Vector3.zero + Vector3.up;
            characterDuck.anim.SetBool("Win", true);
        }

        //khi load lại lần nữa thì lấy thông tin từ datalevel ra để tăng các chỉ số lên
        public override void StartLevel() //==OnEnterIngameAction
        {
            //đã vào trong game rồi, cùng lắm xử lí thêm vài cái kiểu, nhân vật
            //chạm vào cái gì đó rồi có hiệu úng, sự kiện gì đó được bung ra
        }

        public void RandomDuck(GameObject duck, int numberDuck)
        {
            //NavMesh là cái để truy cập vào cùng không gian có thể đi được(đã bake)
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, spawnRadius, NavMesh.AllAreas))
            {
                for (int i = 0; i < numberDuck; i++)
                {
                    // Generate a random position within the spawn radius
                    Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
                    randomPosition.y = 0.0f; // Ensure the position is at ground level
                    // Find the nearest point on the NavMesh to the random position
                    Vector3 spawnPoint = hit.position + randomPosition;
                    // Instantiate an enemy at the spawn point
                    var enemy = Instantiate(duck, spawnPoint, Quaternion.identity);
                    enemy.transform.parent = transform;
                }
            }
            else
            {
                Debug.LogError("No NavMesh found in the scene. Ensure that NavMesh has been baked.");
            }
        }
    }
}