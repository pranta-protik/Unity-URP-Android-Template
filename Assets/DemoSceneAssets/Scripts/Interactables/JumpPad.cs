using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DemoScene.Interactables
{
	public class JumpPad : MonoBehaviour
	{
		public static event UnityAction OnJumpPadInteraction;

		[TabGroup("Jump Settings")][SerializeField] private float _jumpForce = 10f;
		[TabGroup("Jump Settings")][SerializeField] private float _jumpDuration = 0.5f;
		[TabGroup("Visual Settings")][SerializeField] private float _scaleTo = 0.6f;
		[TabGroup("Visual Settings")][SerializeField] private float _scaleTime = 1f;
		[TabGroup("Visual Settings")][SerializeField] private int _vibrato = 5;
		[TabGroup("Visual Settings")][SerializeField, Range(0f, 90f)] private float _randomness = 30f;
		[TabGroup("Visual Settings")][SerializeField] private bool _fadeOut = true;
		[TabGroup("Visual Settings")][EnumToggleButtons, Title("Shake Randomness Mode"), HideLabel][SerializeField] private ShakeRandomnessMode _shakeRandomnessMode = ShakeRandomnessMode.Harmonic;

		private Vector3 _startScale;

		private void Awake() => _startScale = transform.localScale;

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
			{
				OnJumpPadInteraction?.Invoke();

				characterActions.Jump(_jumpForce, _jumpDuration);

				transform.DOKill();
				transform.localScale = _startScale;

				transform.DOShakeScale(_scaleTime, _startScale * _scaleTo, _vibrato, _randomness, _fadeOut, _shakeRandomnessMode);
			}
		}

		private void OnDestroy() => transform.DOKill();
	}
}