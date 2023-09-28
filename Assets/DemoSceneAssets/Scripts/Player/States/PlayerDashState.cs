using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene
{
	public class PlayerDashState : PlayerBaseState
	{
		public class DashStateData
		{
			public CapsuleCollider capsuleCollider;
			public Vector3 dashColliderCenter;
			public float dashColliderHeight;

			public DashStateData(CapsuleCollider capsuleCollider, Vector3 dashColliderCenter, float dashColliderHeight)
			{
				this.capsuleCollider = capsuleCollider;
				this.dashColliderCenter = dashColliderCenter;
				this.dashColliderHeight = dashColliderHeight;
			}
		}

		private readonly DashStateData _dashStateData;

		public PlayerDashState(PlayerController playerController, Animator animator, DashStateData dashStateData) : base(playerController, animator)
		{
			_dashStateData = dashStateData;
		}

		public override void OnEnter()
		{
			// DebugUtils.Log("PlayerDashState.OnEnter");

			_dashStateData.capsuleCollider.center = _dashStateData.dashColliderCenter;
			_dashStateData.capsuleCollider.height = _dashStateData.dashColliderHeight;

			_animator.SetBool(_IsDashing, true);
		}

		public override void FixedUpdate() => _playerController.HandleDash();
	}
}