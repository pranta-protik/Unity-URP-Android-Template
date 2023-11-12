using KBCore.Refs;
using UnityEngine;

namespace DemoScene
{
	public class PlayerAnimationEvent : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Animator _animator;
		
		private bool _enableRootMotion;

		public void EnableRootMotion() => _enableRootMotion = true;
		public void DisableRootMotion() => _enableRootMotion = false;
		
		private void OnAnimatorMove()
		{
			if(!_enableRootMotion) return;
			
			transform.parent.position = _animator.rootPosition;
			transform.parent.rotation = _animator.rootRotation;
		}
	}
}