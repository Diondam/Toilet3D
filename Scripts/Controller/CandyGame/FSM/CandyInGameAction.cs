using Common.FSM;
using Unicorn.FSM;

namespace  Unicorn.Unicorn.Scripts.Controller.CandyGame.FSM
{
    public class CandyInGameAction : InGameAction
    {
        private GameManager _gameManager;  
        public CandyInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            _gameManager = gameManager;
        }
      

        //vào level hiện tại thì làm gì đó trong đây, khởi tạo, tăng độ khó ?
        public override void OnEnter()
        {
            base.OnEnter();
            CandyLevelManager.Instance.Pooling();
        }
        
        //trong lúc chơi có gì cần check như win chẳng hạn ném đây
        public override void OnUpdate()
        {
            base.OnEnter();
            CandyLevelManager.Instance.OnUpdateInGame();
        }
    
        //thoát level hiện tại thì làm gì trong đây, lưu data các thứ
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}