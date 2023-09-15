using UnityEngine;

namespace Toolbox.Utilities
{
	public class ObjectRotator : MonoBehaviour
	{
		[SerializeField] private Vector3 _rotationAxis = Vector3.forward;
		[SerializeField, Range(0f, 5000f)] private float _rotationSpeed = 200f;

		private bool _canRotate = true;

		private void Update()
		{
			if (_canRotate) transform.Rotate(_rotationAxis * (_rotationSpeed * Time.deltaTime));
		}

		public void ToggleRotation(bool enable) => _canRotate = enable;
	}
}