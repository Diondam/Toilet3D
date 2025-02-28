using Common.FSM;
using Unicorn.FSM;

namespace  Unicorn.Unicorn.Scripts.Controller.DriveGame.FSM
{
    public class DriveLobbyAction: LobbyAction
    {
        
        public DriveLobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
           
        }
        public override void OnEnter()
        {
            base.OnEnter();
            GameManager.StartLevel();
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





/*
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
*/