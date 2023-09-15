using DG.Tweening;
using KBCore.Refs;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.UI
{
	public class LevelFailUI : ValidatedMonoBehaviour
	{
		[Header("References")]
		[SerializeField, Anywhere] private Transform _buttonTransform;
		[ShowIf("@_buttonTransform != null")][SerializeField] private float _scaleTo = 1.1f;
		[ShowIf("@_buttonTransform != null")][SerializeField] private float _scaleTime = 0.5f;
		[ShowIf("@_buttonTransform != null")][SerializeField] private Ease _ease = Ease.InOutSine;

		private Vector3 _initialButtonScale;

		private void Awake() => _initialButtonScale = _buttonTransform.localScale;

		private void OnEnable() => _buttonTransform.DOScale(_buttonTransform.localScale * _scaleTo, _scaleTime).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);

		private void OnDisable()
		{
			_buttonTransform.DOKill();
			_buttonTransform.localScale = _initialButtonScale;
		}

		public void OnRetryButtonClick() => SceneUtils.ReloadScene();
	}
}