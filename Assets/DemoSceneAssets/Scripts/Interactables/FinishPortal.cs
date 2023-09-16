using Project.Managers;
using Project.Utilities;
using UnityEngine;

namespace DemoScene.Interactables
{
	public class FinishPortal : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag(ConstUtils.TAG_PLAYER))
			{
				GameManager.Instance.LevelCompleted();
			}
		}
	}
}