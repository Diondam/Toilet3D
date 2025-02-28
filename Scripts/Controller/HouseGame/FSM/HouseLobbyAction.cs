using Common.FSM;
using Unicorn.FSM;

namespace  Unicorn.Unicorn.Scripts.Controller.HouseGame.FSM
{
    public class HouseLobbyAction: LobbyAction
    {
        
        public HouseLobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
           
        }
        //nếu méo làm gì thì nó sẽ chỉ ở trạng thái chờ thôi, như là chờ để tap vào màn hình vậy
        public override void OnEnter()
        {
            base.OnEnter();
            HouseLevelManager.Instance.StoryOnLobby();
            //GameManager.StartLevel();
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