using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace DemoScene.Interactables
{
    public class BouncePad : ValidatedMonoBehaviour
    {
        public static event UnityAction OnBouncePadInteraction;

        [SerializeField, Child] private Animator _animator; 
        [SerializeField] private float _bounceForce = 20f;
        [SerializeField] private float _bounceDuration = 1f;
        private static readonly int _Bounce = Animator.StringToHash("Bounce");

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out ICharacterActions characterActions))
            {
                // If the contact normal is pointing up, the player has collided with the top of the platform
                var contact = other.GetContact(0);
                if (contact.normal.y >= -0.5f) return;
                
                OnBouncePadInteraction?.Invoke();

                _animator.SetTrigger(_Bounce);
                characterActions.Jump(_bounceForce, _bounceDuration);
            }
        }
    }
}