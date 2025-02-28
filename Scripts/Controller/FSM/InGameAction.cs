using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using Unicorn.Examples;
using UnityEngine;

namespace Unicorn.FSM
{
    public class InGameAction : UnicornFSMAction
    {
        public InGameAction(GameManager gameManager, FSMState owner) : base(gameManager, owner)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LevelManager.Instance.StartLevel();
            LevelManager.Instance.DisplayTut();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            LevelManager.Instance.UpdateUI();
        }

        public override void OnExit()
        {
            base.OnExit();
            //SoundManager.Instance.StopSound(SoundManager.GameSound.BGM);
        }
    }
}