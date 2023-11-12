using Project.Unlockables;
using UnityEngine;

namespace DemoScene.Unlockables
{
	public class PlaneHanger : UnlockableItem
	{
		[SerializeField] private GameObject _shield;
		
		protected override void Start()
		{
			base.Start();
			
			_shield.SetActive(!_isUnlocked);
		}

		protected override void UnlockItem()
		{
			base.UnlockItem();
			
			_shield.SetActive(false);
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