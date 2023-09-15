using Project.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI
{
	public class LevelStartTriggerUI : MonoBehaviour, IPointerDownHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			if (GameManager.Instance.CurrentGameState == GameManager.GameState.Waiting) GameManager.Instance.LevelStarted();
		}
	}
}