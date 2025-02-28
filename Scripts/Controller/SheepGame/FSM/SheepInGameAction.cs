using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.SheepGame.FSM
{
    public class SheepInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public SheepInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            _gameManager = gameManager;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            SheepLevelManager.Instance.OnUpdateInGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}