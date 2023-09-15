using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Toolbox.ObjectFader
{
	public class BlockingObjectFader : MonoBehaviour
	{
		[SerializeField] private LayerMask _fadeableObjectLayers;
		[SerializeField, Range(0f, 1f)] private float _fadedAlpha = 0.33f;
		[SerializeField] private float _fadeSpeed = 3f;
		[SerializeField] private bool _retainShadow = true;
		[SerializeField] private Vector3 _positionOffset = Vector3.up;

		private List<FadeableObject> _blockingObjectsList;
		private Dictionary<FadeableObject, Coroutine> _runningCoroutinesDictionary;
		private RaycastHit[] _hitInfos = new RaycastHit[5];
		private Transform _cameraTransform;

		private void Awake()
		{
			_blockingObjectsList = new List<FadeableObject>();
			_runningCoroutinesDictionary = new Dictionary<FadeableObject, Coroutine>();
		}

		private void OnEnable()
		{
			_cameraTransform = Camera.main.transform;
			StartCoroutine(CheckForBlockingObjectsRoutine());
		}

		private IEnumerator CheckForBlockingObjectsRoutine()
		{
			while (true)
			{
				var hitCount = Physics.RaycastNonAlloc(
					_cameraTransform.position,
					(transform.position + _positionOffset - _cameraTransform.position).normalized,
					_hitInfos,
					Vector3.Distance(_cameraTransform.position, transform.position + _positionOffset),
					_fadeableObjectLayers
				);

				if (hitCount > 0)
				{
					for (var i = 0; i < hitCount; i++)
					{
						var blockingObject = GetBlockingObjectFromHitInfo(_hitInfos[i]);

						if (blockingObject != null && !_blockingObjectsList.Contains(blockingObject))
						{
							if (_runningCoroutinesDictionary.TryGetValue(blockingObject, out var runningCoroutine))
							{
								StopCoroutine(runningCoroutine);
								_runningCoroutinesDictionary.Remove(blockingObject);
							}

							_runningCoroutinesDictionary.Add(blockingObject, StartCoroutine(ObjectFadeOutRoutine(blockingObject)));
							_blockingObjectsList.Add(blockingObject);
						}
					}
				}

				FadeInNonBlockingObjects();

				Array.Clear(_hitInfos, 0, _hitInfos.Length);

				yield return null;
			}
		}

		private FadeableObject GetBlockingObjectFromHitInfo(RaycastHit hitInfo)
		{
			return hitInfo.collider != null ? hitInfo.collider.GetComponent<FadeableObject>() : null;
		}

		private IEnumerator ObjectFadeOutRoutine(FadeableObject fadeableObject)
		{
			foreach (var material in fadeableObject.MaterialsList)
			{
				material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.SetInt("_Surface", 1);

				material.renderQueue = (int)RenderQueue.Transparent;

				material.SetShaderPassEnabled("DepthOnly", false);
				material.SetShaderPassEnabled("SHADOWCASTER", _retainShadow);

				material.SetOverrideTag("RenderType", "Transparent");

				material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			}

			var fadeDeltaTime = 0f;

			while (fadeableObject.MaterialsList[0].color.a > _fadedAlpha)
			{
				foreach (var material in fadeableObject.MaterialsList)
				{
					if (material.HasProperty("_BaseColor"))
					{
						material.color = new Color(
							material.color.r,
							material.color.g,
							material.color.b,
							Mathf.Lerp(fadeableObject.InitialAlpha, _fadedAlpha, _fadeSpeed * fadeDeltaTime)
						);
					}
				}

				fadeDeltaTime += Time.deltaTime;

				yield return null;
			}

			if (_runningCoroutinesDictionary.TryGetValue(fadeableObject, out var runningCoroutine))
			{
				StopCoroutine(runningCoroutine);
				_runningCoroutinesDictionary.Remove(fadeableObject);
			}
		}

		private IEnumerator ObjectFadeInRoutine(FadeableObject fadeableObject)
		{
			var fadeDeltaTime = 0f;

			while (fadeableObject.MaterialsList[0].color.a < fadeableObject.InitialAlpha)
			{
				foreach (var material in fadeableObject.MaterialsList)
				{
					if (material.HasProperty("_BaseColor"))
					{
						material.color = new Color(
							material.color.r,
							material.color.g,
							material.color.b,
							Mathf.Lerp(_fadedAlpha, fadeableObject.InitialAlpha, _fadeSpeed * fadeDeltaTime)
						);
					}
				}

				fadeDeltaTime += Time.deltaTime;

				yield return null;
			}

			foreach (var material in fadeableObject.MaterialsList)
			{
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.SetInt("_Surface", 0);

				material.renderQueue = (int)RenderQueue.Geometry;

				material.SetShaderPassEnabled("DepthOnly", true);
				material.SetShaderPassEnabled("SHADOWCASTER", true);

				material.SetOverrideTag("RenderType", "Opaque");

				material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			}

			if (_runningCoroutinesDictionary.TryGetValue(fadeableObject, out var runningCoroutine))
			{
				StopCoroutine(runningCoroutine);
				_runningCoroutinesDictionary.Remove(fadeableObject);
			}
		}

		private void FadeInNonBlockingObjects()
		{
			var nonBlockingObjectsList = new List<FadeableObject>(_blockingObjectsList.Count);

			foreach (var blockingObject in _blockingObjectsList)
			{
				var isBlocking = false;

				for (var i = 0; i < _hitInfos.Length; i++)
				{
					var testObject = GetBlockingObjectFromHitInfo(_hitInfos[i]);

					if (testObject != null && testObject == blockingObject)
					{
						isBlocking = true;
						break;
					}
				}

				if (!isBlocking)
				{
					if (_runningCoroutinesDictionary.TryGetValue(blockingObject, out var runningCoroutine))
					{
						StopCoroutine(runningCoroutine);
						_runningCoroutinesDictionary.Remove(blockingObject);
					}

					_runningCoroutinesDictionary.Add(blockingObject, StartCoroutine(ObjectFadeInRoutine(blockingObject)));
					nonBlockingObjectsList.Add(blockingObject);
				}
			}

			foreach (var nonBlockingObject in nonBlockingObjectsList)
			{
				_blockingObjectsList.Remove(nonBlockingObject);
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawRay(
				Camera.main.transform.position,
				(transform.position + _positionOffset - Camera.main.transform.position).normalized *
				Vector3.Distance(Camera.main.transform.position, transform.position + _positionOffset)
				);

			Gizmos.color = Color.white;
		}
#endif
	}
}