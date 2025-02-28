using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.BoatGame.FSM
{
    public class BoatInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public BoatInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
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
            BoatLevelManager.Instance.OnUpdateInGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}