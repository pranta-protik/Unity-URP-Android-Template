using UnityEngine;

namespace DemoScene.Unlockables
{
	public class ProtectiveShield : MonoBehaviour
	{
		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
			{
				characterActions.Stun();
				// OnJumpPadInteraction?.Invoke();
			}
		}
	}
}