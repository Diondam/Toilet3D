using Common.FSM;
using Unicorn.FSM;

namespace  Unicorn.Unicorn.Scripts.Controller.SheepGame.FSM
{
    public class SheepEndGameAction: EndgameAction
    {
        public SheepEndGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
            
        }
        public override void OnEnter()
        {
            base.OnEnter();
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