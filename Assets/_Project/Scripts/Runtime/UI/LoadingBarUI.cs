using KBCore.Refs;
using Project.Utilities;
using Sirenix.OdinInspector;
using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class LoadingBarUI : ValidatedMonoBehaviour
    {
        [Header("References")] [SerializeField, Anywhere] private Image _fillMask;
        [ShowIf("@_fillMask != null")] [SerializeField, Range(0f, 10f)] private float _loadingDuration = 3f;

        private CountdownTimer _loadingTimer;

        private void Awake()
        {
            _fillMask.fillAmount = 0f;

            _loadingTimer = new CountdownTimer(_loadingDuration);
            _loadingTimer.OnTimerStop += () => { SceneUtils.LoadSpecificScene((int)SceneIndex.GAME); };
        }

        private void Start() => _loadingTimer.Start();

        private void Update()
        {
            _loadingTimer.Tick(Time.deltaTime);
            _fillMask.fillAmount = _loadingTimer.Progress;
        }
    }
}