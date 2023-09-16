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
			public float colliderResetDuration;

			public LocomotionStateData(CapsuleCollider capsuleCollider, Vector3 defaultColliderCenter, float defaultColliderHeight, float colliderResetDuration)
			{
				this.capsuleCollider = capsuleCollider;
				this.defaultColliderCenter = defaultColliderCenter;
				this.defaultColliderHeight = defaultColliderHeight;
				this.colliderResetDuration = colliderResetDuration;
			}
		}

		private const float LERPING_THRESHOLD = 0.01f;
		private readonly LocomotionStateData _locomotionStateData;

		public PlayerLocomotionState(PlayerController playerController, Animator animator, LocomotionStateData locomotionStateData) : base(playerController, animator)
		{
			_locomotionStateData = locomotionStateData;
		}

		public override void OnEnter()
		{
			// DebugUtils.Log("PlayerLocomotionState.OnEnter");

			_animator.SetBool(_IsJumping, false);
			_animator.SetBool(_IsDashing, false);
			_animator.SetBool(_IsCrouching, false);
		}

		public override void Update()
		{
			if (Vector3.Distance(_locomotionStateData.capsuleCollider.center, _locomotionStateData.defaultColliderCenter) > LERPING_THRESHOLD)
			{
				_locomotionStateData.capsuleCollider.center = Vector3.Lerp(_locomotionStateData.capsuleCollider.center, _locomotionStateData.defaultColliderCenter, _locomotionStateData.colliderResetDuration * Time.deltaTime);
			}
			else
			{
				_locomotionStateData.capsuleCollider.center = _locomotionStateData.defaultColliderCenter;
			}

			if (Mathf.Abs(_locomotionStateData.capsuleCollider.height - _locomotionStateData.defaultColliderHeight) > LERPING_THRESHOLD)
			{
				_locomotionStateData.capsuleCollider.height = Mathf.Lerp(_locomotionStateData.capsuleCollider.height, _locomotionStateData.defaultColliderHeight, _locomotionStateData.colliderResetDuration * Time.deltaTime);
			}
			else
			{
				_locomotionStateData.capsuleCollider.height = _locomotionStateData.defaultColliderHeight;
			}
		}

		public override void FixedUpdate()
		{
			_playerController.HandleMovement();
			_playerController.HandleDrop();
		}
	}
}