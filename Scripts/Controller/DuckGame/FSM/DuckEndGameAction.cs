using Common.FSM;
using Unicorn.FSM;
using Unicorn.Utilities;

namespace Unicorn.Unicorn.Scripts.Controller.DuckGame.FSM
{
    public class DuckEndGameAction: EndgameAction
    {
        private GameManager _gameManager;
        public DuckEndGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            _gameManager = gameManager;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            UltimateJoystick.DisableJoystick("Movement");
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {
            base.OnExit();
        }

       
    }
}