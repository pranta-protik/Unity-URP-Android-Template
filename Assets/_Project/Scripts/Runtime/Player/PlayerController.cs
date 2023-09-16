using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using Project.Managers;
using Project.Persistent.SaveSystem;
using Sirenix.OdinInspector;
using Toolbox.SM;
using Toolbox.Utilities;
using UnityEngine;

namespace Project
{
	public class PlayerController : ValidatedMonoBehaviour, ICharacterActions, IDataPersistence
	{
		private const float ZERO_F = 0f;
		private const float ONE_F = 1f;
		private static readonly int _Speed = Animator.StringToHash("Speed");

		[TabGroup("References")][SerializeField, Self] private Rigidbody _rigidbody;
		[TabGroup("References")][SerializeField, Self] private CapsuleCollider _capsuleCollider;
		[TabGroup("References")][SerializeField, Child] private Animator _animator;
		[TabGroup("References")][SerializeField, Self] private GroundChecker _groundChecker;
		[TabGroup("References")][SerializeField, Self] private CeilingChecker _ceilingChecker;
		[TabGroup("References")][SerializeField, Scene] private Joystick _joystick;
		[TabGroup("References")][SerializeField, Anywhere] private CinemachineVirtualCamera _playerVCam;

		[TabGroup("Movement Settings")][SerializeField] private float _moveSpeed = 300f;
		[TabGroup("Movement Settings")][SerializeField] private float _rotationSpeed = 500f;
		[TabGroup("Movement Settings")][SerializeField] private float _stoppingSpeed = 3f;
		[TabGroup("Movement Settings")][SerializeField] private float _smoothTime = 0.2f;
		[TabGroup("Movement Settings")][SerializeField] private float _colliderResetDuration = 0.5f;

		[TabGroup("Jump Settings")][SerializeField] private float _jumpForce = 10f;
		[TabGroup("Jump Settings")][SerializeField] private float _jumpDuration = 0.5f;
		[TabGroup("Jump Settings")][SerializeField] private float _gravityMultiplier = 3f;

		[TabGroup("Dash Settings")][SerializeField] private float _dashForce = 1.5f;
		[TabGroup("Dash Settings")][SerializeField] private float _dashDuration = 0.7f;
		[TabGroup("Dash Settings")][SerializeField] private Vector3 _dashColliderCenter = new(0f, 0.7f, 0f);
		[TabGroup("Dash Settings")][SerializeField] private float _dashColliderHeight = 1.4f;

		[TabGroup("Crouch Settings")][SerializeField] private float _crouchDecelaration = 0.5f;
		[TabGroup("Crouch Settings")][SerializeField] private Vector3 _crouchColliderCenter = new(0f, 0.7f, 0f);
		[TabGroup("Crouch Settings")][SerializeField] private float _crouchColliderHeight = 1.4f;

		private float _currentSpeed;
		private float _velocity;
		private float _jumpVelocity;
		private float _dashVelocity = 1f;
		private float _crouchVelocity = 1f;
		private Vector3 _defaultColliderCenter;
		private float _defaultColliderHeight;
		private Vector3 _moveDir;
		private List<Timer> _timersList;
		private CountdownTimer _jumpTimer;
		private CountdownTimer _dashTimer;
		private StateMachine _stateMachine;

		[ShowInInspector, DisplayAsString, Title("Rigidbody Velocity"), HideLabel] public float RigidbodyVelocity => _rigidbody.velocity.magnitude;

		private void Awake()
		{
			_playerVCam.Follow = transform;
			_rigidbody.freezeRotation = true;

			_defaultColliderCenter = _capsuleCollider.center;
			_defaultColliderHeight = _capsuleCollider.height;

			SetupTimers();
			SetupStateMachine();
		}

		private void SetupTimers()
		{
			_jumpTimer = new CountdownTimer(_jumpDuration);
			_jumpTimer.OnTimerStart += () => _jumpVelocity = _jumpForce;

			_dashTimer = new CountdownTimer(_dashDuration);
			_dashTimer.OnTimerStart += () => _dashVelocity = _dashForce;

			_timersList = new List<Timer>(2) { _jumpTimer, _dashTimer };
		}

		private void SetupStateMachine()
		{
			_stateMachine = new StateMachine();

			// Declare States
			var locomotionState = new PlayerLocomotionState(this, _animator, new PlayerLocomotionState.LocomotionStateData(
				_capsuleCollider,
				_defaultColliderCenter,
				_defaultColliderHeight,
				_colliderResetDuration));

			var jumpState = new PlayerJumpState(this, _animator);

			var dashState = new PlayerDashState(this, _animator, new PlayerDashState.DashStateData(
				_capsuleCollider,
				_dashColliderCenter,
				_dashColliderHeight));

			var crouchState = new PlayerCrouchState(this, _animator, new PlayerCrouchState.CrouchStateData(
				_capsuleCollider,
				_crouchColliderCenter,
				_crouchColliderHeight,
				_crouchDecelaration
				));

			// Declate Transitions
			At(locomotionState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));
			At(locomotionState, dashState, new FuncPredicate(() => _dashTimer.IsRunning));
			At(locomotionState, crouchState, new FuncPredicate(() => _ceilingChecker.IsTouchingCeiling));
			At(dashState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));
			At(dashState, crouchState, new FuncPredicate(() => !_dashTimer.IsRunning && _ceilingChecker.IsTouchingCeiling));

			Any(locomotionState, new FuncPredicate(() => _groundChecker.IsGrounded && !_jumpTimer.IsRunning && !_dashTimer.IsRunning && !_ceilingChecker.IsTouchingCeiling));

			// Set initial state
			_stateMachine.SetState(locomotionState);
		}

		private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
		private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

		private void Update()
		{
			if (GameManager.Instance.CurrentGameState == GameManager.GameState.Running)
			{
				_moveDir = new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y);
			}
			else
			{
				_moveDir = Vector3.Lerp(_moveDir, Vector3.zero, _stoppingSpeed * Time.deltaTime);
			}

			_stateMachine.Update();

			HandleTimers();
			UpdateAnimator();
		}

		private void HandleTimers()
		{
			foreach (var timer in _timersList) timer.Tick(Time.deltaTime);
		}

		private void UpdateAnimator() => _animator.SetFloat(_Speed, _currentSpeed);

		private void FixedUpdate() => _stateMachine.FixedUpdate();

		public void HandleMovement()
		{
			if (_moveDir.magnitude > ZERO_F)
			{
				HandleRotation();
				HandleHorizontalMovement();
				SmoothSpeed(_moveDir.magnitude);
			}
			else
			{
				SmoothSpeed(ZERO_F);

				// Reset horizontal velocity for a snappy stop
				_rigidbody.velocity = new Vector3(ZERO_F, _rigidbody.velocity.y, ZERO_F);
			}
		}

		private void HandleRotation()
		{
			if (_moveDir != Vector3.zero)
			{
				var targetRotation = Quaternion.LookRotation(_moveDir);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
			}
		}

		private void HandleHorizontalMovement()
		{
			var velocity = _crouchVelocity * _moveSpeed * Time.fixedDeltaTime * _moveDir;
			_rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
		}

		public void HandleDrop()
		{
			if (!_groundChecker.IsGrounded) _rigidbody.AddForce(Physics.gravity * _gravityMultiplier, ForceMode.Force);
		}

		public void Jump() => _jumpTimer.Start();

		public void HandleJump()
		{
			if (!_jumpTimer.IsRunning && _groundChecker.IsGrounded)
			{
				_jumpVelocity = ZERO_F;
				_jumpTimer.Stop();
				return;
			}

			if (!_jumpTimer.IsRunning)
			{
				// Gravity takes over
				_jumpVelocity += Physics.gravity.y * _gravityMultiplier * Time.fixedDeltaTime;
			}

			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpVelocity, _rigidbody.velocity.z);
		}

		public void Dash() => _dashTimer.Start();

		public void HandleDash()
		{
			var normalizedMoveDir = _moveDir.normalized;
			var velocity = _dashVelocity * _moveSpeed * Time.fixedDeltaTime * normalizedMoveDir;

			if (normalizedMoveDir.magnitude > ZERO_F)
			{
				HandleRotation();
				SmoothSpeed(normalizedMoveDir.magnitude);
			}
			else
			{
				// Keep applying dash velocity even if there's no input from player
				velocity = _dashVelocity * _moveSpeed * Time.fixedDeltaTime * transform.forward;
				SmoothSpeed(ONE_F);
			}

			_rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
		}

		private void SmoothSpeed(float value) => _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, _smoothTime);

		public void SetCrouchVelocity(float velocity) => _crouchVelocity = velocity;

		public void LoadData(GameData gameData)
		{
			if (gameData.playerPositionDictionary.TryGetValue(SceneUtils.GetActiveSceneIndex(), out var playerPosition))
			{
				transform.position = playerPosition;
			}
		}

		public void SaveData(GameData gameData)
		{
			if (gameData.playerPositionDictionary.ContainsKey(SceneUtils.GetActiveSceneIndex()))
			{
				gameData.playerPositionDictionary.Remove(SceneUtils.GetActiveSceneIndex());
			}

			if (GameManager.Instance.IsGameOver()) return;

			gameData.playerPositionDictionary.Add(SceneUtils.GetActiveSceneIndex(), transform.position);
		}
	}
}