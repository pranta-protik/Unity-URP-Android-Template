using Cinemachine;
using KBCore.Refs;
using Project.Managers;
using Sirenix.OdinInspector;
using Toolbox.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace DemoScene
{
	public class PlaneController : ValidatedMonoBehaviour
	{
		[TabGroup("References")][SerializeField, Scene] private Joystick _joystick;
		[TabGroup("References")][SerializeField, Anywhere] private CinemachineVirtualCamera _planeVCam;
		[TabGroup("References")][SerializeField, Anywhere] private Transform _leftAileron;
		[TabGroup("References")][SerializeField, Anywhere] private Transform _rightAileron;
		[TabGroup("References")][SerializeField, Anywhere] private Transform _leftElevator;
		[TabGroup("References")][SerializeField, Anywhere] private Transform _rightElevator;

		[TabGroup("Movement Settings")][SerializeField] private Vector2 _movementLimit = new(8f, 6f);
		[TabGroup("Movement Settings")][SerializeField] private float _movementRange = 5f;
		[TabGroup("Movement Settings")][SerializeField] private float _movementSpeed = 1.5f;
		[TabGroup("Movement Settings")][SerializeField] private float _maxRoll = 15f;
		[TabGroup("Movement Settings")][SerializeField] private float _rollSpeed = 5f;
		[TabGroup("Movement Settings")][SerializeField] private float _maxPitch = 10f;
		[TabGroup("Movement Settings")][SerializeField] private float _pitchSpeed = 5f;
		[TabGroup("Movement Settings")][SerializeField] private float _aileronMaxRotation = 60f;
		[TabGroup("Movement Settings")][SerializeField] private float _elevatorMaxRotation = 30f;

		public UnityEvent OnEjected;

		private Vector3 _targetPosition;
		private float _roll;
		private float _pitch;

		private void Awake()
		{
			_planeVCam.Follow = transform;
			_planeVCam.LookAt = transform;

			_targetPosition = transform.localPosition;
		}

		private void Update()
		{
			if (GameManager.Instance.CurrentGameState == GameManager.GameState.Running)
			{
				HandleMovement();
			}
		}

		private void HandleMovement()
		{
			_targetPosition.x += _joystick.Direction.x * _movementSpeed * _movementRange * Time.deltaTime;
			_targetPosition.y += _joystick.Direction.y * _movementSpeed * _movementRange * Time.deltaTime;

			_targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit.x, _movementLimit.x);
			_targetPosition.y = Mathf.Clamp(_targetPosition.y, -_movementLimit.y, _movementLimit.y);

			transform.localPosition = _targetPosition;

			_roll = Mathf.Lerp(_roll, -_joystick.Direction.x * _maxRoll, _rollSpeed * Time.deltaTime);
			_pitch = Mathf.Lerp(_pitch, -_joystick.Direction.y * _maxPitch, _pitchSpeed * Time.deltaTime);

			transform.localRotation = Quaternion.Euler(_pitch, transform.localEulerAngles.y, _roll);

			// Wing flaps rotation

			var aileronRotation = _roll.Remap(-_maxRoll, _maxRoll, -_aileronMaxRotation, _aileronMaxRotation);

			_leftAileron.localRotation = Quaternion.Euler(aileronRotation, _leftAileron.localEulerAngles.y, _leftAileron.localEulerAngles.z);
			_rightAileron.localRotation = Quaternion.Euler(-aileronRotation, _rightAileron.localEulerAngles.y, _rightAileron.localEulerAngles.z);

			// Tail flaps rotation

			var elevatorRotation = _pitch.Remap(-_maxPitch, _maxPitch, -_elevatorMaxRotation, _elevatorMaxRotation);

			_leftElevator.localRotation = Quaternion.Euler(-elevatorRotation, _leftElevator.localEulerAngles.y, _leftElevator.localEulerAngles.z);
			_rightElevator.localRotation = Quaternion.Euler(-elevatorRotation, _rightElevator.localEulerAngles.y, _rightElevator.localEulerAngles.z);
		}

		private void ResetOrientation()
		{
			_roll = 0f;
			_pitch = 0f;
			_targetPosition = Vector3.zero;

			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		public void OnEjectButtonClick()
		{
			OnEjected?.Invoke();
			ResetOrientation();
			ControlSwitcher.Instance.SwitchToPlayerControl();
		}
	}
}