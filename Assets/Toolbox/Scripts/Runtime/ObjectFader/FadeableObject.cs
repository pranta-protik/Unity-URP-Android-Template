using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.ObjectFader
{
	public class FadeableObject : MonoBehaviour, IEquatable<FadeableObject>
	{
		[SerializeField] private List<Renderer> _renderersList = new();
		private Vector3 _position;

		public List<Material> MaterialsList { get; private set; }
		public float InitialAlpha { get; private set; }

		private void Awake()
		{
			MaterialsList = new List<Material>();

			_position = transform.position;

			if (_renderersList.Count == 0)
			{
				_renderersList.AddRange(GetComponentsInChildren<Renderer>());
			}

			foreach (var renderer in _renderersList)
			{
				MaterialsList.AddRange(renderer.materials);
			}

			InitialAlpha = MaterialsList[0].color.a;
		}

		public bool Equals(FadeableObject other)
		{
			return _position.Equals(other._position);
		}

		public override int GetHashCode()
		{
			return _position.GetHashCode();
		}
	}
}