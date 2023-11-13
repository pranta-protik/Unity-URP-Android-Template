using KBCore.Refs;
using Project.Unlockables;
using UnityEngine;

namespace DemoScene.Unlockables
{
	public class PlaneHanger : UnlockableItem
	{
		[SerializeField, Child] private ProtectiveShield _protectiveShield;
		
		protected override void Start()
		{
			base.Start();
			
			_protectiveShield.ToggleStatus(!_isUnlocked);
		}

		protected override void UnlockItem()
		{
			base.UnlockItem();
			
			_protectiveShield.ToggleStatus(false);
		}

		public override void Interact(Transform interactor)
		{
			if (interactor)
			{
				ControlSwitcher.Instance.SwitchToPlaneControl();
				interactor.position = _idlingSpot.position;
			}
		}
	}
}