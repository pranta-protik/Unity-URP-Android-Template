using Toolbox.Utilities;
using UnityEngine;

namespace Project
{
	public class PlayerCrouchState : PlayerBaseState
	{
		public class CrouchStateData
		{
			public CapsuleCollider capsuleCollider;
			public Vector3 crouchColliderCenter;
			public float crouchColliderHeight;
			public float crouchDeceleration;

			public CrouchStateData(CapsuleCollider capsuleCollider, Vector3 crouchColliderCenter, float crouchColliderHeight, float crouchDeceleration)
			{
				this.capsuleCollider = capsuleCollider;
				this.crouchColliderCenter = crouchColliderCenter;
				this.crouchColliderHeight = crouchColliderHeight;
				this.crouchDeceleration = crouchDeceleration;
			}
		}

		private readonly CrouchStateData _crouchStateData;

		public PlayerCrouchState(PlayerController playerController, Animator animator, CrouchStateData crouchStateData) : base(playerController, animator)
		{
			_crouchStateData = crouchStateData;
		}

		public override void OnEnter()
		{
			// DebugUtils.Log("PlayerCrouchState.OnEnter");

			_playerController.SetCrouchVelocity(_crouchStateData.crouchDeceleration);

			_crouchStateData.capsuleCollider.center = _crouchStateData.crouchColliderCenter;
			_crouchStateData.capsuleCollider.height = _crouchStateData.crouchColliderHeight;

			_animator.SetBool(_IsCrouching, true);
		}

		public override void FixedUpdate() => _playerController.HandleMovement();

		public override void OnExit()
		{
			// DebugUtils.Log("PlayerCrouchState.OnExit");
			_playerController.SetCrouchVelocity(1f);
		}
	}
}