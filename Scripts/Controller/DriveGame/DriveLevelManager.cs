using Unicorn.Unicorn.Scripts.Controller.DriveGame.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.DriveGame
{
    public class DriveLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
       
        /*************************************************** game  ******************************************************************/
        
        public new static DriveLevelManager Instance => LevelManager.Instance as DriveLevelManager;
        DriveLobbyAction DrivelobbyAction;
        DriveInGameAction DriveinGameAction;
        DriveEndGameAction DriveendGameAction;
        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            DrivelobbyAction = new DriveLobbyAction(GameManager.Instance, lobbyGameState);
            DriveinGameAction = new DriveInGameAction(GameManager.Instance, inGameState);
            DriveendGameAction = new DriveEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(DrivelobbyAction);
            inGameState.AddAction(DriveinGameAction);
            endGameState.AddAction(DriveendGameAction);
        }
        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(DrivelobbyAction);
         
            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(DriveinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(DriveendGameAction);
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
        //     characterDrive.transform.position = Vector3.zero + Vector3.up;
        //     characterDrive.transform.eulerAngles = new Vector3(0, 180, 0);
        //     characterDrive.GetComponent<Animator>().SetBool("Win", true);
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