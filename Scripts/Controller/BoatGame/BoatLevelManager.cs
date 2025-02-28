using Unicorn.Unicorn.Scripts.Controller.BoatGame.FSM;
namespace Unicorn.Unicorn.Scripts.Controller.BoatGame
{
    public class BoatLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
       
        /*************************************************** game  ******************************************************************/
        
        public new static BoatLevelManager Instance => LevelManager.Instance as BoatLevelManager;
        BoatLobbyAction BoatlobbyAction;
        BoatInGameAction BoatinGameAction;
        BoatEndGameAction BoatendGameAction;
        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            BoatlobbyAction = new BoatLobbyAction(GameManager.Instance, lobbyGameState);
            BoatinGameAction = new BoatInGameAction(GameManager.Instance, inGameState);
            BoatendGameAction = new BoatEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(BoatlobbyAction);
            inGameState.AddAction(BoatinGameAction);
            endGameState.AddAction(BoatendGameAction);
        }
        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(BoatlobbyAction);
         
            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(BoatinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(BoatendGameAction);
        }
        public override void StartLevel()
        {
           
        }
        
        public void OnUpdateInGame()
        {
           
        }

        public override void Win()
        {
            base.Win();
            tut.transform.GetChild(3).gameObject.SetActive(false);
            tut.transform.GetChild(4).gameObject.SetActive(false);
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
        //     characterBoat.transform.position = Vector3.zero + Vector3.up;
        //     characterBoat.transform.eulerAngles = new Vector3(0, 180, 0);
        //     characterBoat.GetComponent<Animator>().SetBool("Win", true);
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