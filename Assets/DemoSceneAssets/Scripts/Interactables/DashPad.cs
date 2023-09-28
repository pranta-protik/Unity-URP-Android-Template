using UnityEngine;
using UnityEngine.Events;

namespace DemoScene.Interactables
{
	public class DashPad : MonoBehaviour
	{
		public static event UnityAction OnDashPadInteraction;

		[SerializeField] private float _dashForce = 1.5f;
		[SerializeField] private float _dashDuration = 0.7f;

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
			{
				OnDashPadInteraction?.Invoke();
				characterActions.Dash(_dashForce, _dashDuration);
			}
		}
	}
}