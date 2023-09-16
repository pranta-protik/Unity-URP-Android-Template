using UnityEngine;

namespace Project
{
	public class GroundChecker : MonoBehaviour
	{
		[SerializeField] private float _groundDistance = 0.35f;
		[SerializeField] private Vector3 _originOffset = new(0f, 0.05f, 0f);
		[SerializeField] private float _sphereRadius = 0.02f;
		[SerializeField] private LayerMask _groundLayers;

		public bool IsGrounded { get; private set; }

		private void Update()
		{
			IsGrounded = Physics.SphereCast(transform.position + _originOffset, _sphereRadius, Vector3.down, out _, _groundDistance, _groundLayers);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + _originOffset, _sphereRadius);
			Gizmos.DrawWireSphere(transform.position + _originOffset + Vector3.down * _groundDistance, _sphereRadius);
			Gizmos.color = Color.white;
		}
#endif
	}
}