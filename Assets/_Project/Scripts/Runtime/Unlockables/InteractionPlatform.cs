using KBCore.Refs;
using Project.Utilities;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Unlockables
{
    public class InteractionPlatform : ValidatedMonoBehaviour
    {
        [TabGroup("References")] [SerializeField, Parent] private InterfaceRef<IInteractable> _interactable;
        [TabGroup("References")] [SerializeField, Anywhere] private Image _loadingBarFill;
        [TabGroup("Progression Settings")] [SerializeField] private float _preparationDuration = 2f;

        private Transform _interactor;
        private CountdownTimer _preparationTimer;

        private void Awake()
        {
            _preparationTimer = new CountdownTimer(_preparationDuration);
        }

        private void Start()
        {
            _preparationTimer.OnTimerStop += () =>
            {
                _interactable.Value.Interact(_interactor);
                _loadingBarFill.fillAmount = 0f;
            };

            _loadingBarFill.fillAmount = 0f;
        }

        private void Update()
        {
            _preparationTimer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(ConstUtils.TAG_PLAYER)) return;

            _interactor = other.transform;
            _preparationTimer.Start();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag(ConstUtils.TAG_PLAYER)) return;

            _loadingBarFill.fillAmount = _preparationTimer.Progress;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag(ConstUtils.TAG_PLAYER)) return;

            _interactor = null;
            _preparationTimer.Pause();
            _loadingBarFill.fillAmount = 0f;
        }
    }
}