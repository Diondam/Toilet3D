using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.DuckGame.FSM
{
    public class DuckInGameAction : InGameAction
    {

       
        public DuckInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            
        }
        
        //vào level hiện tại thì làm gì đó trong đây, khởi tạo, tăng độ khó ?
        public override void OnEnter()
        {
            base.OnEnter();
            
        }
        
        //trong lúc chơi có gì cần check như win chẳng hạn ném đây
        public override void OnUpdate()
        {
            base.OnUpdate();
            DuckLevelManager.Instance.OnUpdateInGame();
        }

      
          
        //thoát level hiện tại thì làm gì trong đây, lưu data các thứ
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}