using Project.Utilities;
using UnityEngine;

namespace DemoScene.Interactables
{
	public class PlatformCollisionHandler : MonoBehaviour
	{
		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag(ConstUtils.TAG_PLAYER))
			{
				// If the contact normal is pointing up, the player has collided with the top of the platform
				var contact = other.GetContact(0);
				if (contact.normal.y >= -0.5f) return;

				other.transform.SetParent(transform);
			}
		}

		private void OnCollisionExit(Collision other)
		{
			if (other.gameObject.CompareTag(ConstUtils.TAG_PLAYER))
			{
				other.transform.SetParent(null);
			}
		}
	}
}