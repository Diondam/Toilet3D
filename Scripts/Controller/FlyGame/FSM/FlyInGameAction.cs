using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.FlyGame.FSM
{
    public class FlyInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public FlyInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
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
            FlyLevelManager.Instance.OnUpdateInGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}