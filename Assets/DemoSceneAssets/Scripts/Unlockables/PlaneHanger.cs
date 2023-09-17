using Project;
using Project.Unlockables;
using UnityEngine;

namespace DemoScene.Unlockables
{
	public class PlaneHanger : UnlockableItem
	{
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