using DG.Tweening;
using KBCore.Refs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.UI
{
	public class LevelStartUI : ValidatedMonoBehaviour
	{
		[Header("Hand Settings")]
		[SerializeField, Anywhere] private Transform _handTransform;
		[ShowIf("@_handTransform != null")][SerializeField] private Vector3 _moveTo = Vector3.zero;
		[ShowIf("@_handTransform != null")][SerializeField] private float _moveTime = 1f;
		[ShowIf("@_handTransform != null")][SerializeField] private Ease _moveEase = Ease.InOutSine;

		[Header("Text Settings")]
		[SerializeField, Anywhere] private Transform _textTransform;
		[ShowIf("@_textTransform != null")][SerializeField] private float _scaleTo = 1.2f;
		[ShowIf("@_textTransform != null")][SerializeField] private float _scaleTime = 0.5f;
		[ShowIf("@_textTransform != null")][SerializeField] private Ease _scaleEase = Ease.InOutSine;

		private Vector3 _startPosition;
		private Vector3 _startScale;

		private void Awake()
		{
			_startPosition = _handTransform.localPosition;
			_startScale = _textTransform.localScale;
		}

		private void OnEnable()
		{
			_handTransform.DOLocalMove(_moveTo, _moveTime).SetEase(_moveEase).SetLoops(-1, LoopType.Yoyo);
			_textTransform.DOScale(_textTransform.localScale * _scaleTo, _scaleTime).SetEase(_scaleEase).SetLoops(-1, LoopType.Yoyo);
		}

		private void OnDisable()
		{
			_handTransform.DOKill();
			_handTransform.localPosition = _startPosition;
			_textTransform.DOKill();
			_textTransform.localScale = _startScale;
		}
	}
}