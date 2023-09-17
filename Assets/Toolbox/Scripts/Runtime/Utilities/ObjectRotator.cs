using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolbox.Utilities
{
	public class ObjectRotator : MonoBehaviour
	{
		private enum RotationType
		{
			Fixed,
			Random
		}

		[EnumToggleButtons][SerializeField] private RotationType _rotationType = RotationType.Fixed;
		[EnableIf("@_rotationType != RotationType.Random")][SerializeField] private Vector3 _rotationAxis = Vector3.forward;
		[SerializeField, Range(0f, 5000f)] private float _rotationSpeed = 200f;

		private bool _canRotate = true;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (_rotationType == RotationType.Random)
			{
				_rotationAxis = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
			}
		}
#endif

		private void Update()
		{
			if (_canRotate) transform.Rotate(_rotationAxis * (_rotationSpeed * Time.deltaTime));
		}

		public void ToggleRotation(bool enable) => _canRotate = enable;
	}
}