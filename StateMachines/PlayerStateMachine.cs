using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public static Action<bool> OnPlayerRun;
    public static Action<bool> OnPlayerIdle;
    public static Action OnPlayerJump;
    public static Action<bool> OnPlayerJumpCancel;

    [Header("Jumping"), SerializeField] private float _maxJumpHeight = 1f;
    [SerializeField] private float _maxJumpTime = 0.5f;

    [Header("Falling"), SerializeField] private float _fallClampSpeed = -20f;

    [Header("Rotation Speed"), SerializeField]
    private float movementSpeed = 5.0f;

    [Header("Rotation Speed"), SerializeField]
    private float rotationFactorPerFrame = 1.0f;

    [Header("Ground Check"), SerializeField]
    private LayerMask _layerGround;

    private PlayerInput _playerInput;
    private CharacterController _characterController;

    private Vector2 _currMovementInput;
    private Vector3 _currMovement;
    private Vector3 _applidedMovement;
    private bool _isMovementPressed;
    private bool _isJumpPressed;
    private float _initialJumpVelocity;
    private bool _isJumping;
    private bool _requireNewJumpPress;
    private int _jumpCount;

    #region Constants

    private float _gravity = -9.8f;
    private float _groundedGravity = -0.5f;

    #endregion

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    #region Getters and Setters

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public CharacterController CharacterController => _characterController;

    public bool IsJumpPressed => _isJumpPressed;

    public bool IsJumping
    {
        get => _isJumping;
        set => _isJumping = value;
    }

    public Vector2 CurrentMovementInput => _currMovementInput;
    
    public float CurrMovementY
    {
        get => _currMovement.y;
        set => _currMovement.y = value;
    }

    public float AppliedMovementX
    {
        get => _applidedMovement.x;
        set => _applidedMovement.x = value;
    }

    public float AppliedMovementY
    {
        get => _applidedMovement.y;
        set => _applidedMovement.y = value;
    }

    public float AppliedMovementZ
    {
        get => _applidedMovement.z;
        set => _applidedMovement.z = value;
    }

    public float InitialJumpVelocity => _initialJumpVelocity;

    public bool IsMovementPressed => _isMovementPressed;

    public float GroundedGravity => _groundedGravity;

    public float Gravity => _gravity;

    public float FallClampSpeed => _fallClampSpeed;

    public bool RequireNewJumpPress
    {
        get => _requireNewJumpPress;
        set => _requireNewJumpPress = value;
    }

    #endregion

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        SetupJumpVariables();
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
        _playerInput.CharacterControls.Move.started -= OnMovementInput;
        _playerInput.CharacterControls.Move.canceled -= OnMovementInput;
        _playerInput.CharacterControls.Move.performed -= OnMovementInput;
        _playerInput.CharacterControls.Jump.started -= OnJump;
        _playerInput.CharacterControls.Jump.canceled -= OnJump;
    } 

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currMovementInput = context.ReadValue<Vector2>();
        _currMovement.x = _currMovementInput.x;
        _currMovement.z = _currMovementInput.y;
        _isMovementPressed = _currMovementInput.x != 0f || _currMovementInput.y != 0f;

        if (_isJumping) return;
        OnPlayerRun?.Invoke(_isMovementPressed);
        OnPlayerIdle?.Invoke(_isMovementPressed);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    private void SetupJumpVariables()
    {
        // parabola formula
        //resource: https://www.youtube.com/watch?v=h2r3_KjChf4 from time: 10:00
        float timeToApex = _maxJumpTime / 2f;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
    }

    private void Update()
    {
        HandleRotation();
        _currentState.UpdateStates();
        HandleMovement();
    }

    private void HandleMovement() => _characterController.Move(_applidedMovement * movementSpeed * Time.deltaTime);

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation =
                Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    public bool IsGrounded => Physics.Raycast(transform.position, Vector3.down,
        _characterController.height * 0.1f, _layerGround);
}