using Toolbox.Utilities;
using UnityEngine;

namespace Project
{
	public class PlayerJumpState : PlayerBaseState
	{
		public PlayerJumpState(PlayerController playerController, Animator animator) : base(playerController, animator)
		{
		}

		public override void OnEnter()
		{
			DebugUtils.Log("PlayerJumpState.OnEnter");
			_animator.SetBool(_IsJumping, true);
		}

		public override void FixedUpdate()
		{
			_playerController.HandleJump();
			_playerController.HandleMovement();
		}
	}
}