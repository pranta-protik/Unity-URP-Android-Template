using DG.Tweening;
using Project;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DemoScene.Interactables
{
	public class JumpPad : MonoBehaviour
	{
		public static event UnityAction OnJumpPadInteraction;

		[SerializeField] private float _scaleTo = 0.6f;
		[SerializeField] private float _scaleTime = 1f;
		[SerializeField] private int _vibrato = 5;
		[SerializeField, Range(0f, 90f)] private float _randomness = 30f;
		[SerializeField] private bool _fadeOut = true;
		[EnumToggleButtons, Title("Shake Randomness Mode"), HideLabel][SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Harmonic;

		private Vector3 _startScale;

		private void Awake() => _startScale = transform.localScale;

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
			{
				OnJumpPadInteraction?.Invoke();

				characterActions.Jump();

				transform.DOKill();
				transform.localScale = _startScale;

				transform.DOShakeScale(_scaleTime, _startScale * _scaleTo, _vibrato, _randomness, _fadeOut, _shakeRandomnessMode);
			}
		}

		private void OnDestroy() => transform.DOKill();
	}
}