using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Toolbox.Utilities
{
	public static class HapticUtils
	{
		public enum HapticScale
		{
			Light = HapticTypes.LightImpact,
			Medium = HapticTypes.MediumImpact,
			Heavy = HapticTypes.HeavyImpact
		}

		private const string PREF_HAPTIC_STATUS = "HapticStatus";
		private static int _HapticStatus;
		private static bool _EnableLog;

		static HapticUtils() => _HapticStatus = PlayerPrefs.GetInt(PREF_HAPTIC_STATUS, 1);

		public static void SetHapticLevel(HapticScale hapticScale)
		{
			if (_HapticStatus == 0) return;
			MMVibrationManager.Haptic((HapticTypes)hapticScale);

			if (_EnableLog) DebugUtils.Log($"{hapticScale} Impact!");
		}

		public static void SetHapticStatus(bool enable)
		{
			_HapticStatus = enable ? 1 : 0;
			PlayerPrefs.SetInt(PREF_HAPTIC_STATUS, _HapticStatus);
		}

		public static void SetLogStatus(bool enable)
		{
			_EnableLog = enable;
		}
	}
}