using Common.FSM;
using Unicorn.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.HouseGame.FSM
{
    public class HouseInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public HouseInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            _gameManager = gameManager;
        }


        //vào level hiện tại thì làm gì đó trong đây, khởi tạo, tăng độ khó ?
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Ingame of House");
            HouseLevelManager.Instance.OnStart();
        }


        //trong lúc chơi có gì cần check như win chẳng hạn ném đây
        public override void OnUpdate()
        {
            base.OnUpdate();
            HouseLevelManager.Instance.OnUpdate();
        }


        //thoát level hiện tại thì làm gì trong đây, lưu data các thứ
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}