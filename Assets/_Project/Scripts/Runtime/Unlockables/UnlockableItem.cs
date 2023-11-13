using KBCore.Refs;
using Project.IS;
using Project.Managers;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;

namespace Project.Unlockables
{
    public abstract class UnlockableItem : MonoBehaviour, IUnlockable, IInteractable, IDataPersistence
    {
        [TabGroup("References")] [SerializeField, Child] private PaymentPlatform _paymentPlatform;
        [TabGroup("References")] [SerializeField, Child] private InteractionPlatform _interactionPlatform;
        [TabGroup("References")] [InlineEditor] [SerializeField, Anywhere] private InventoryItemDataSO _itemData;
        [TabGroup("References")] [SerializeField, Anywhere] protected Transform _idlingSpot;

        [TabGroup("Data Settings")] [InlineButton("GenerateGUID")] [SerializeField, LabelText("Id (Unique)")] private string _id;
        private void GenerateGUID() => _id = System.Guid.NewGuid().ToString();
        [TabGroup("Data Settings")] [SerializeField] private int _unlockCost = 100;
        [TabGroup("Data Settings")] [SerializeField] private float _unlockDelay = 0.5f;

        private int _depositedAmount;
        protected bool _isUnlocked;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_id == "") DebugUtils.LogError("Unique Id is required.");
            this.ValidateRefs();
        }
#endif

        protected virtual void Start()
        {
            _isUnlocked = _depositedAmount >= _unlockCost;

            _paymentPlatform.gameObject.SetActive(!_isUnlocked);
            _interactionPlatform.gameObject.SetActive(_isUnlocked);
        }

        public void Deposit(int amount)
        {
            if (CanDeposit())
            {
                if (amount > InventorySystem.Instance.Get(_itemData).StackSize)
                {
                    amount = InventorySystem.Instance.Get(_itemData).StackSize;
                }

                _depositedAmount += amount;
                InventorySystem.Instance.Remove(_itemData, amount);

                if (_depositedAmount >= _unlockCost)
                {
                    Invoke(nameof(UnlockItem), _unlockDelay);
                }
            }
        }

        protected virtual void UnlockItem()
        {
            _paymentPlatform.gameObject.SetActive(false);
            _interactionPlatform.gameObject.SetActive(true);
        }

        public bool CanDeposit()
        {
            if (_depositedAmount < _unlockCost && InventorySystem.Instance.Get(_itemData) != null)
            {
                if (InventorySystem.Instance.Get(_itemData).StackSize > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public float GetDepositedAmountNormalized() => (float)_depositedAmount / _unlockCost;

        public abstract void Interact(Transform interactor);

        public void LoadData(GameData gameData)
        {
            if (gameData.unlockableItemDictionary.TryGetValue(_id, out var depositedAmount))
            {
                _depositedAmount = depositedAmount;
            }
        }

        public void SaveData(GameData gameData)
        {
            if (gameData.unlockableItemDictionary.ContainsKey(_id))
            {
                gameData.unlockableItemDictionary.Remove(_id);
            }

            if (GameManager.Instance.IsGameOver()) return;

            gameData.unlockableItemDictionary.Add(_id, _depositedAmount);
        }

        protected virtual void OnDestroy()
        {
        }
    }
}