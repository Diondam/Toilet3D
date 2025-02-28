using Unicorn.Unicorn.Scripts.Controller.JumpGame.FSM;
namespace Unicorn.Unicorn.Scripts.Controller.JumpGame
{
    public class JumpLevelManager : LevelManager
    {
        /*************************************************** game  ******************************************************************/
       
        /*************************************************** game  ******************************************************************/
        
        public new static JumpLevelManager Instance => LevelManager.Instance as JumpLevelManager;
        JumpLobbyAction JumplobbyAction;
        JumpInGameAction JumpinGameAction;
        JumpEndGameAction JumpendGameAction;
        protected override void Awake()
        {
            base.Awake();
            var lobbyGameState = GameManager.Instance.GameStateController.LobbyGameState;
            var inGameState = GameManager.Instance.GameStateController.InGameState;
            var endGameState = GameManager.Instance.GameStateController.EndGameState;

            JumplobbyAction = new JumpLobbyAction(GameManager.Instance, lobbyGameState);
            JumpinGameAction = new JumpInGameAction(GameManager.Instance, inGameState);
            JumpendGameAction = new JumpEndGameAction(GameManager.Instance, endGameState);

            lobbyGameState.AddAction(JumplobbyAction);
            inGameState.AddAction(JumpinGameAction);
            endGameState.AddAction(JumpendGameAction);
        }
        private void OnDestroy()
        {
            var inGameState = GameManager.Instance.GameStateController.LobbyGameState;
            inGameState.RemoveAction(JumplobbyAction);
         
            var preparationState = GameManager.Instance.GameStateController.InGameState;
            preparationState.RemoveAction(JumpinGameAction);

            var endGameState = GameManager.Instance.GameStateController.EndGameState;
            endGameState.RemoveAction(JumpendGameAction);
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
        //     characterJump.transform.position = Vector3.zero + Vector3.up;
        //     characterJump.transform.eulerAngles = new Vector3(0, 180, 0);
        //     characterJump.GetComponent<Animator>().SetBool("Win", true);
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