using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace Project.InputSystem
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Input Reader", fileName = "NewInputReader")]
	public class InputReaderSO : ScriptableObject, IPlayerActions
	{
		public event UnityAction<Vector2> Move = delegate { };
		public event UnityAction<bool> Jump = delegate { };

		private PlayerInputActions _inputActions;

		public Vector2 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

		private void OnEnable()
		{
			if (_inputActions == null)
			{
				_inputActions = new PlayerInputActions();
				_inputActions.Player.SetCallbacks(this);
			}
		}

		public void EnablePlayerInputActions() => _inputActions.Enable();

		public void OnMove(InputAction.CallbackContext context)
		{
			Move.Invoke(context.ReadValue<Vector2>());
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					Jump.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					Jump.Invoke(false);
					break;
			}
		}
	}
}