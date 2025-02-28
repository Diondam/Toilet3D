using System.Collections;
using DG.Tweening;
using TMPro;
using Unicorn.Unicorn.Scripts.Controller.HouseGame.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.HouseGame
{
    public class HouseLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
        public Transform[] objectsToSwap;
        public Transform mooring1;
        public Transform mooring2;
        public float swapInterval = 3.0f; // Time interval for swapping

        public int loop = 3;

        //private bool swapped = false;
        private bool isEndSwap = false;
        public GameObject pumpkin;
        public GameObject slashEffectPrefab;
        public float delayWin = 1f;
        public GameObject character;
        /*************************************************** game  ******************************************************************/


        public new static HouseLevelManager Instance => LevelManager.Instance as HouseLevelManager;
        HouseLobbyAction HouselobbyAction;
        HouseInGameAction HouseinGameAction;
        HouseEndGameAction HouseendGameAction;

        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            HouselobbyAction = new HouseLobbyAction(GameManager.Instance, lobbyGameState);
            HouseinGameAction = new HouseInGameAction(GameManager.Instance, inGameState);
            HouseendGameAction = new HouseEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(HouselobbyAction);
            inGameState.AddAction(HouseinGameAction);
            endGameState.AddAction(HouseendGameAction);
        }

        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(HouselobbyAction);
            HouselobbyAction.OnExit();

            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(HouseinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(HouseendGameAction);
        }

        public override void StartLevel()
        {
        }

        public override void SetDifficulty()
        {
        }

        public override void StoryOnLobby()
        {
            base.StoryOnLobby();
        }


        public Transform cam1;
        public Transform cam2;

        public override IEnumerator AnimOnStart()
        {
            var orinPositionos = camera.transform.position;
            var OrinRotate = camera.transform.eulerAngles;
            //anim 1

            //begin position camera
            camera.transform.position = cam1.position;
            camera.transform.eulerAngles = cam1.eulerAngles;
            yield return new WaitForSeconds(1f);
            
            //anim 2

            //quay camera
            camera.transform.DOMove(cam2.position, 1f);
            camera.transform.DORotate(cam2.eulerAngles, 1f);
            yield return new WaitForSeconds(1f);

            //quay về camera gốc
            camera.transform.DOMove(orinPositionos, 1f);
            camera.transform.DORotate(OrinRotate, 1f);
            yield return new WaitForSeconds(1f);
            PlayGamePlay.gameObject.SetActive(true);
        }


        public void OnStart()
        {
            StartCoroutine(SetUp());
        }

        private float KeyPosY;
        private Quaternion pumpQuaternion;

        IEnumerator SetUp()
        {
            KeyPosY = pumpkin.transform.position.y;
            pumpQuaternion = pumpkin.transform.rotation;
            int randomIndex = Random.Range(0, objectsToSwap.Length);
            pumpkin.transform.SetParent(objectsToSwap[randomIndex], true);
            pumpkin.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(1f);
            StartCoroutine(Up(objectsToSwap[0]));
            StartCoroutine(Up(objectsToSwap[1]));
            StartCoroutine(Up(objectsToSwap[2]));
            yield return new WaitForSeconds(1f);
            StartCoroutine(Down(objectsToSwap[0]));
            StartCoroutine(Down(objectsToSwap[1]));
            StartCoroutine(Down(objectsToSwap[2]));
            yield return new WaitForSeconds(1f);
            //rotationProgress = 0;
            StartCoroutine("SwapPhrase");
        }

        public void OnUpdate()
        {
            pumpkin.transform.position =
                new Vector3(pumpkin.transform.position.x, KeyPosY, pumpkin.transform.position.z);
            pumpkin.transform.rotation = pumpQuaternion;
            if (isEndSwap)
            {
                if (Input.GetMouseButtonDown(0)) // Kiểm tra xem có bao nhiêu ngón tay đang chạm vào màn hình
                {
                    print("Đã chạm màn hình");
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        print("Đã chạm :" + hit.transform.gameObject.name);
                        // Nếu dò ray chạm vào đối tượng này
                        if (hit.transform.gameObject.tag == "House")
                        {
                            print("QUAY");
                            // StartCoroutine(RotateBack(hit.transform));
                            StartCoroutine(Up(hit.transform));
                            if (pumpkin.transform.parent == hit.transform)
                            {
                                // GameObject effect = Instantiate(slashEffectPrefab, pumpkin.transform.position,
                                //     Quaternion.identity);
                                // effect.transform.parent = transform;
                                StartCoroutine(DelayWin());
                            }
                            else
                            {
                                character.GetComponent<Animator>().SetBool("Lose", true);
                                EndGame(LevelResult.Lose);
                            }
                        }
                    }
                }
            }
        }

        IEnumerator DelayWin()
        {
            character.transform.rotation = Quaternion.Euler(0, 0, 0);
            character.GetComponent<Animator>().SetBool("Win", true);
            yield return new WaitForSeconds(delayWin);
            Win();
        }

        public float openTime = 0;

        IEnumerator Up(Transform objectToOpen)
        {
            while (objectToOpen.position.y < 1.5f)
            {
                objectToOpen.position += Vector3.up * Time.deltaTime;
                objectToOpen.Rotate(Vector3.right * -150 * Time.deltaTime);
                yield return null;
            }
        }

        IEnumerator Down(Transform objectToOpen)
        {
            while (objectToOpen.position.y > KeyPosY)
            {
                objectToOpen.position += Vector3.down * Time.deltaTime;
                objectToOpen.Rotate(Vector3.right * 150 * Time.deltaTime);
                yield return null;
            }
        }

        // public float rotationProgress = 0;
        // public float rotationSpeed = 1.0f;
        //
        // IEnumerator RotateBack(Transform objectToBack)
        // {
        //     var initialRotation = objectToBack.rotation;
        //     var targetRotation = Quaternion.Euler(objectToBack.eulerAngles + new Vector3(0, 180, 0));
        //     while (rotationProgress <= 1)
        //     {
        //         //print("ROTATE");
        //         rotationProgress += Time.deltaTime * rotationSpeed;
        //         objectToBack.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);
        //
        //         if (rotationProgress >= 1)
        //         {
        //             objectToBack.rotation = targetRotation; // Đảm bảo đối tượng đã quay đúng 180 độ
        //         }
        //
        //         yield return null;
        //     }
        //
        //     //rotationProgress = 0;
        // }

        IEnumerator SwapPhrase()
        {
            for (int i = 0; i < loop; i++)
            {
                int index1 = Random.Range(0, objectsToSwap.Length);
                int index2 = (index1 + 1) % objectsToSwap.Length;
                StartCoroutine(Swap(objectsToSwap[index1], objectsToSwap[index2], swapInterval));
                yield return new WaitForSeconds(swapInterval);
            }

            isEndSwap = true;
        }

        IEnumerator Swap(Transform a, Transform b, float duration)
        {
            float elapsedTime = 0.0f;
            Vector3 aPos = a.position;
            Vector3 bPos = b.position;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                a.position = Quadratic(aPos, mooring1.position, bPos, t);
                b.position = Quadratic(bPos, mooring2.position, aPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            a.position = bPos;
            b.position = aPos;
        }

        public Vector3 Quadratic(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 ab = Vector3.Lerp(a, b, t);
            Vector3 bc = Vector3.Lerp(b, c, t);
            return Vector3.Lerp(ab, bc, t);
        }
    }
}