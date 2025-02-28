using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unicorn.Unicorn.Scripts.Character.Fly;
using Unicorn.Unicorn.Scripts.Controller.FlyGame.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.Unicorn.Scripts.Controller.FlyGame
{
    public class FlyLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/

        public Slider slider;
        public Animator charAnim;
        public GameObject rocket;

        public TextMeshProUGUI textControl;
        /*************************************************** game  ******************************************************************/

        public new static FlyLevelManager Instance => LevelManager.Instance as FlyLevelManager;
        FlyLobbyAction FlylobbyAction;
        FlyInGameAction FlyinGameAction;
        FlyEndGameAction FlyendGameAction;

        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            FlylobbyAction = new FlyLobbyAction(GameManager.Instance, lobbyGameState);
            FlyinGameAction = new FlyInGameAction(GameManager.Instance, inGameState);
            FlyendGameAction = new FlyEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(FlylobbyAction);
            inGameState.AddAction(FlyinGameAction);
            endGameState.AddAction(FlyendGameAction);
        }

        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(FlylobbyAction);

            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(FlyinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(FlyendGameAction);
        }

        public override void StartLevel()
        {
        }

        private bool isLaunchOk;
        public float amountTap = 2;
        private bool canlaunch = true;

        public void OnUpdateInGame()
        {
            if (Input.GetMouseButtonDown(0) && canlaunch) // Kiểm tra xem có bao nhiêu ngón tay đang chạm vào màn hình
            {
                charAnim.SetBool("Pump", true);
            }

            //tap to fill force for rocket
            if (Input.GetMouseButton(0) && canlaunch)
            {
                slider.value += 0.1f * Time.deltaTime * amountTap; // Tăng giá trị slider
            }

            if (Input.GetMouseButtonUp(0))
            {
                charAnim.SetBool("Pump", false);
                if (0.7f <= slider.value && slider.value <= 0.8f && !isLaunchOk && canlaunch)
                {
                    canlaunch = false;
                    MoveAlongPath();
                    StartCoroutine(AnimOnStart2());
                    //charAnim.SetBool("RocketOK", true);
                    slider.gameObject.SetActive(false);
                }

                if (slider.value > 0.8f)
                {
                    EndGame(LevelResult.Lose);
                }
            }

            //when rocket move to right position
            if (isLaunchOk)
            {
                if (!active)
                {
                    StartCoroutine(ActiveText());
                }
                rocket.GetComponent<Testy>().enabled = true;
            }
        }

        private bool active;

        IEnumerator ActiveText()
        {
            active = true;
            textControl.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            textControl.gameObject.SetActive(false);
        }

        public List<Transform> pathPoints;

        void MoveAlongPath()
        {
            // Convert Transform list to Vector3 list
            List<Vector3> pathVectors = pathPoints.Select(transform => transform.position).ToList();

            // Move the object along the path
            rocket.transform.DOPath(pathVectors.ToArray(), flyDuration, PathType.CatmullRom)
                .SetLookAt(0.01f)
                .SetDelay(1f)
                .OnComplete(() =>
                {
                    isLaunchOk = true;
                    rocket.transform.rotation = pathPoints[pathPoints.Count].rotation;
                });
        }

        public float flyDuration = 8f;
        public Transform cam1;
        public Transform cam2;
        public Camera tempCam;

        public override IEnumerator AnimOnStart()
        {
            tempCam.depth = 10;

            tempCam.transform.position = cam1.position;
            tempCam.transform.eulerAngles = cam1.eulerAngles;
            yield return new WaitForSeconds(1f);

            tempCam.transform.DOMove(cam2.position, 1f);
            tempCam.transform.DORotate(cam2.eulerAngles, 1f);
            yield return new WaitForSeconds(1f);

            PlayGamePlay.gameObject.SetActive(true);
        }

        public IEnumerator AnimOnStart2()
        {
            tempCam.transform.DOMove(camera.transform.position, 1f);
            tempCam.transform.DORotate(camera.transform.eulerAngles, 1f);
            yield return new WaitForSeconds(1f);
            tempCam.depth = -10;
            charAnim.SetBool("Launch", true);
        }


        // public override void UpdateUI()
        // {
        //     if (timeGamePlay >= 10)
        //     {
        //         timePlay.text = "00:" + (int)timeGamePlay;
        //     }
        //     else
        //     {
        //         timePlay.text = "00:0" + (int)timeGamePlay;
        //     }
        // }

        // public override void Win()
        // {
        //     Invoke("SetCamForAnim", 0.5f);
        //     base.Win();
        // }
        // public void SetCamForAnim()
        // {
        //     characterFly.transform.position = Vector3.zero + Vector3.up;
        //     characterFly.transform.eulerAngles = new Vector3(0, 180, 0);
        //     characterFly.GetComponent<Animator>().SetBool("Win", true);
        // }


        public override void SetDifficulty()
        {
        }

        public void End()
        {
            EndGame(LevelResult.Lose);
        }
    }
}