using Unicorn.Unicorn.Scripts.Controller.SheepGame.FSM;
namespace Unicorn.Unicorn.Scripts.Controller.SheepGame
{
    public class SheepLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
       
        /*************************************************** game  ******************************************************************/
        
        public new static SheepLevelManager Instance => LevelManager.Instance as SheepLevelManager;
        SheepLobbyAction SheeplobbyAction;
        SheepInGameAction SheepinGameAction;
        SheepEndGameAction SheependGameAction;
        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            SheeplobbyAction = new SheepLobbyAction(GameManager.Instance, lobbyGameState);
            SheepinGameAction = new SheepInGameAction(GameManager.Instance, inGameState);
            SheependGameAction = new SheepEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(SheeplobbyAction);
            inGameState.AddAction(SheepinGameAction);
            endGameState.AddAction(SheependGameAction);
        }
        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(SheeplobbyAction);
         
            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(SheepinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(SheependGameAction);
        }
        public override void StartLevel()
        {
           
        }
        
        public void OnUpdateInGame()
        {
           
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
        //     characterSheep.transform.position = Vector3.zero + Vector3.up;
        //     characterSheep.transform.eulerAngles = new Vector3(0, 180, 0);
        //     characterSheep.GetComponent<Animator>().SetBool("Win", true);
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