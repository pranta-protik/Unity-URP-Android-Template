using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene
{
    public class PlayerStunnedState : PlayerBaseState
    { 
        public PlayerStunnedState(PlayerController playerController, Animator animator) : base(playerController, animator)
        {
        }
        
        public override void OnEnter()
        {
            // DebugUtils.Log("PlayerStunnedState.OnEnter");
            _animator.SetBool(_IsStunned, true);
        }
    }
}