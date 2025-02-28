using Common.FSM;
using Unicorn.FSM;

namespace  Unicorn.Unicorn.Scripts.Controller.FlyGame.FSM
{
    public class FlyLobbyAction: LobbyAction
    {
        
        public FlyLobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
           
        }
        public override void OnEnter()
        {
            base.OnEnter();
            FlyLevelManager.Instance.StoryOnLobby();
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