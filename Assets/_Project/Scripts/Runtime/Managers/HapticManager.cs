using DemoScene.Interactables;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Managers
{
	public class HapticManager : MonoBehaviour
	{
		private enum LogState
		{
			Enable,
			Disable
		}

		[EnumToggleButtons, Title("Haptic Log Status"), HideLabel][SerializeField] private LogState _logState = LogState.Enable;

		private void Awake() => HapticUtils.SetLogStatus(_logState == LogState.Enable);

		private void Start()
		{
			JumpPad.OnJumpPadInteraction += JumpPad_OnJumpPadInteraction;
			DashPad.OnDashPadInteraction += DashPad_OnDashPadInteraction;
		}

		private void OnDestroy()
		{
			JumpPad.OnJumpPadInteraction -= JumpPad_OnJumpPadInteraction;
			DashPad.OnDashPadInteraction -= DashPad_OnDashPadInteraction;
		}

		private void JumpPad_OnJumpPadInteraction() => HapticUtils.SetHapticLevel(HapticUtils.HapticScale.Medium);
		private void DashPad_OnDashPadInteraction() => HapticUtils.SetHapticLevel(HapticUtils.HapticScale.Medium);
	}
}