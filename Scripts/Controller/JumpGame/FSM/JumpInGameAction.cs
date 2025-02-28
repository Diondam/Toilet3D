using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.JumpGame.FSM
{
    public class JumpInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public JumpInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
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
            JumpLevelManager.Instance.OnUpdateInGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}