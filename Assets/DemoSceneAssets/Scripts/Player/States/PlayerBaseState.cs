using Toolbox.SM;
using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene
{
	public abstract class PlayerBaseState : IState
	{
		protected static readonly int _IsJumping = Animator.StringToHash("IsJumping");
		protected static readonly int _IsDashing = Animator.StringToHash("IsDashing");
		protected static readonly int _IsCrouching = Animator.StringToHash("IsCrouching");
		protected static readonly int _IsStunned = Animator.StringToHash("IsStunned");

		protected readonly PlayerController _playerController;
		protected readonly Animator _animator;

		protected PlayerBaseState(PlayerController playerController, Animator animator)
		{
			_playerController = playerController;
			_animator = animator;
		}

		public virtual void Update()
		{
		}

		public virtual void FixedUpdate()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit()
		{
			// DebugUtils.Log("PlayerBaseState.OnExit");
		}
	}
}