using Common.FSM;
using Unicorn.FSM;
using UnityEngine;

namespace Unicorn.Unicorn.Scripts.Controller.SurfGame.FSM
{
    public class SurfLobbyAction : LobbyAction
    {
        public SurfLobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        //nếu méo làm gì thì nó sẽ chỉ ở trạng thái chờ thôi, như là chờ để tap vào màn hình vậy
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("BẮT ĐẦU GAME SURF : ");
            SurfLevelManager.Instance.Pooling();
            SurfLevelManager.Instance.PoolingMap();
            if (GameManager.Instance.CurrentLevel == 3)
            {
                SurfLevelManager.Instance.PoolingTrap();
            }

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