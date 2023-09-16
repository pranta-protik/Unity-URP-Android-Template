using Toolbox.Utilities;
using UnityEngine;

namespace Project
{
	public class PlayerLocomotionState : PlayerBaseState
	{
		public class LocomotionStateData
		{
			public CapsuleCollider capsuleCollider;
			public Vector3 defaultColliderCenter;
			public float defaultColliderHeight;

			public LocomotionStateData(CapsuleCollider capsuleCollider, Vector3 defaultColliderCenter, float defaultColliderHeight)
			{
				this.capsuleCollider = capsuleCollider;
				this.defaultColliderCenter = defaultColliderCenter;
				this.defaultColliderHeight = defaultColliderHeight;
			}
		}

		private readonly LocomotionStateData _locomotionStateData;

		public PlayerLocomotionState(PlayerController playerController, Animator animator, LocomotionStateData locomotionStateData) : base(playerController, animator)
		{
			_locomotionStateData = locomotionStateData;
		}

		public override void OnEnter()
		{
			DebugUtils.Log("PlayerLocomotionState.OnEnter");

			_locomotionStateData.capsuleCollider.center = _locomotionStateData.defaultColliderCenter;
			_locomotionStateData.capsuleCollider.height = _locomotionStateData.defaultColliderHeight;

			_animator.SetBool(_IsJumping, false);
			_animator.SetBool(_IsDashing, false);
			_animator.SetBool(_IsCrouching, false);
		}

		public override void FixedUpdate()
		{
			_playerController.HandleMovement();
			_playerController.HandleDrop();
		}
	}
}