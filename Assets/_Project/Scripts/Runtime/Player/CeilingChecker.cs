using Sirenix.OdinInspector;
using UnityEngine;

namespace Project
{
	public class CeilingChecker : MonoBehaviour
	{
		[SerializeField] private float _ceilingDistance = 1f;
		[SerializeField] private Vector3 _originOffset = new(0f, 1.35f, 0f);
		[SerializeField] private float _sphereRadius = 0.02f;
		[SerializeField] private LayerMask _ceilingLayers;
		[ShowInInspector, DisplayAsString] public bool IsTouchingCeiling { get; private set; }

		private void Update()
		{
			IsTouchingCeiling = Physics.SphereCast(transform.position + _originOffset, _sphereRadius, Vector3.up, out _, _ceilingDistance, _ceilingLayers);
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + _originOffset, _sphereRadius);
			Gizmos.DrawWireSphere(transform.position + _originOffset + Vector3.up * _ceilingDistance, _sphereRadius);
			Gizmos.color = Color.white;
		}
#endif
	}
}