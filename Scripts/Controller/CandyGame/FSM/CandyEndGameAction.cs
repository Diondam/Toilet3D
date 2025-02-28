using Common.FSM;
using Unicorn.FSM;
using Unicorn.Utilities;

namespace Unicorn.Unicorn.Scripts.Controller.CandyGame.FSM
{
    public class CandyEndGameAction: EndgameAction
    {
        public CandyEndGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            
        }
        public override void OnEnter()
        {
            base.OnEnter();
            //DuckLevelManager.Instance.joyStick.enabled = false;
            UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
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