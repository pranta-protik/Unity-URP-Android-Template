using UnityEngine;
using UnityEngine.Rendering;

namespace Toolbox.Settings
{
	[CreateAssetMenu(fileName = "NewLightingSettings", menuName = "ScriptableObjects/Lighting Settings")]
	public class LightingSettingsSO : ScriptableObject
	{
		public Material skyboxMaterial;
		public AmbientMode ambientMode;
		[ColorUsage(true, true)] public Color ambientColor;
	}
}