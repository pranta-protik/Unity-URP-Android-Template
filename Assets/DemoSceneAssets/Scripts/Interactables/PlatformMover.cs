using DG.Tweening;
using UnityEngine;

namespace DemoScene.Interactables
{
	public class PlatformMover : MonoBehaviour
	{
		[SerializeField] private Vector3 _moveTo = Vector3.zero;
		[SerializeField] private float _moveTime = 2f;
		[SerializeField] private Ease _ease = Ease.InOutQuad;

		private void Start() => transform.DOLocalMove(_moveTo, _moveTime).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
		private void OnDestroy() => transform.DOKill();
	}
}