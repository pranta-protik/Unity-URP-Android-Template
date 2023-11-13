using KBCore.Refs;
using Toolbox.Utilities;
using UnityEngine;

namespace DemoScene.Unlockables
{
    public class ProtectiveShield : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Collider _collider;
        [SerializeField, Child] private Renderer _renderer;
        [SerializeField] private float _dissolveDuration = 1f;

        private CountdownTimer _dissolveTimer;
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int _Dissolve = Shader.PropertyToID("_Dissolve");

        private void Awake()
        {
            _dissolveTimer = new CountdownTimer(_dissolveDuration);
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            _dissolveTimer.OnTimerStop += () => { gameObject.SetActive(false); };
        }

        private void Update()
        {
            _dissolveTimer.Tick(Time.deltaTime);

            if (!_dissolveTimer.IsRunning) return;
            _materialPropertyBlock.SetFloat(_Dissolve, _dissolveTimer.Progress);
            _renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
            {
                characterActions.Stun();
            }
        }

        public void ToggleStatus(bool enable)
        {
            if (enable) return;

            _dissolveTimer.Start();
            _collider.enabled = false;
        }
    }
}