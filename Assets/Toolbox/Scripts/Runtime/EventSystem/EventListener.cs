using Toolbox.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ES
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventChannelSO<T> _eventChannel;
        [SerializeField] private UnityEvent<T> _unityEvent;

        private void OnValidate()
        {
            if (_eventChannel == null) DebugUtils.LogError("Event channel missing.");
        }

        protected void Awake()
        {
            _eventChannel.Register(this);
        }

        private void OnDestroy()
        {
            _eventChannel.Unregister(this);
        }

        public void Raise(T value)
        {
            _unityEvent?.Invoke(value);
        }
    }

    public class EventListener : EventListener<Empty> { }
}
