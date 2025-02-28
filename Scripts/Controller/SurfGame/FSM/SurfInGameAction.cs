using Common.FSM;
using Unicorn.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.SurfGame.FSM
{
    public class SurfInGameAction : InGameAction
    {
        private GameManager _gameManager;  
        public SurfInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            _gameManager = gameManager;
        }
      

        //vào level hiện tại thì làm gì đó trong đây, khởi tạo, tăng độ khó ?
        public override void OnEnter()
        {
            base.OnEnter();
           Debug.Log("Ingame Surf");
        }

        
        //trong lúc chơi có gì cần check như win chẳng hạn ném đây
        public override void OnUpdate()
        {
            base.OnUpdate();
            SurfLevelManager.Instance.OnUpdateInGame();
     
        }

      
          
        //thoát level hiện tại thì làm gì trong đây, lưu data các thứ
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}