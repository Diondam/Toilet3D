using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.DriveGame.FSM
{
    public class DriveInGameAction : InGameAction
    {
        private GameManager _gameManager;

        public DriveInGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
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
            DriveLevelManager.Instance.OnUpdateInGame();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}