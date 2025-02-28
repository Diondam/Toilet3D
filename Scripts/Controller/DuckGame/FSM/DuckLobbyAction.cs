using Common.FSM;
using Unicorn.FSM;

namespace Unicorn.Unicorn.Scripts.Controller.DuckGame.FSM
{
    public class DuckLobbyAction : LobbyAction
    {
        private DuckLevelManager levelMana => DuckLevelManager.Instance;

        public DuckLobbyAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        //nếu méo làm gì thì nó sẽ chỉ ở trạng thái chờ thôi, như là chờ để tap vào màn hình vậy
        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnter()
        {
            base.OnEnter();
            GameManager.StartLevel();
            levelMana.RandomDuck(levelMana.enemyPrefab, levelMana.numberOfEnemies);
            /*if (GameManager.Instance.CurrentLevel <= 4)
            {
                levelMana.RandomDuck(levelMana.enemyPrefab, levelMana.numberOfEnemies);
            }

            if (GameManager.Instance.CurrentLevel >= 3)
            {
                levelMana.RandomStuffMesh(levelMana.numberOfTrap, levelMana.Trap);
            }

            if (GameManager.Instance.CurrentLevel >= 4)
            {
                levelMana.RandomStuffMesh(levelMana.numberOfUselessItem, levelMana.uselessItem);
            }

            if (GameManager.Instance.CurrentLevel == 5 && GameManager.Instance.CurrentLevel == 6)
            {
                levelMana.RandomDuck(levelMana.enemyPrefab, levelMana.numberOfEnemies);
                levelMana.RandomDuck(levelMana.HPGhost1, levelMana.numberHPGhost1);
            }

            if (GameManager.Instance.CurrentLevel >= 5 && GameManager.Instance.CurrentLevel >= 6)
            {
               levelMana.RandomStuffMesh(levelMana.SOSnumberitem, levelMana.RandomItemInList(levelMana.speedOrSwordItem));
                levelMana.RandomStuffMesh(levelMana.numberHPItem, levelMana.HPItem);
            }

            if (GameManager.Instance.CurrentLevel >= 7)
            {
                levelMana.RandomDuck(levelMana.HPGhost1, levelMana.numberHPGhost1);
                levelMana.RandomDuck(levelMana.HPGhost2, levelMana.numberHPGhost2);
            }

            if (GameManager.Instance.CurrentLevel >= 8)
            {
                levelMana.RandomDuck(levelMana.HPGhost3, levelMana.numberHPGhost3);
            }

            if (GameManager.Instance.CurrentLevel >= 9)
            {
                levelMana.RandomStuffMesh(levelMana.numberBoomDuck, levelMana.BoomDuck);
            }*/
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