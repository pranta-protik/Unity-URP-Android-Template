using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Toolbox.Settings
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Lighting Settings", fileName = "NewLightingSettings")]
	public class LightingSettingsSO : ScriptableObject
	{
		public Material skyboxMaterial;
		[EnumToggleButtons] public AmbientMode ambientMode;
		[ColorUsage(true, true)] public Color ambientColor;
	}
}