using SeraphUtil;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Camera _mainCamera;
    [SerializeField] PhotonView _photonView;
    [SerializeField] Spaceship _spaceship;

    [field: Header("Movement Variables")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1000;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 10;

   
    
    // Input System
    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private bool _isGamepad = true;
    private InputSystem.PlayerActions Input { get { return _inputSystem.Player; } }

    // Movement
    private Vector2 _lookDirection;
    
    // Weapons - Shortcuts from spaceship script
    private Lazgun PrimaryWeapon { get { return _spaceship.PrimaryWeapon; } }
    private Weapon SpecialWeapon { get { return _spaceship.SpecialWeapon; } }
    private int SpecialAmmo { get { return _spaceship.SpecialAmmo; } }
    private float PrimaryHeat { get { return _spaceship.PrimaryHeat; } }



    // Gizmos feedback
    private Vector2 _acceleration = Vector3.zero;
    private Vector2 _prevVelocity = Vector3.zero;


    private void Awake()
    {
        _inputSystem = new InputSystem();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        Input.Enable();
        
    }
    private void OnDisable()
    {
        Input.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();

        _acceleration = _rigidbody2D.velocity - _prevVelocity;
        _prevVelocity = _rigidbody2D.velocity;
    }
    
    private void Movement()
    {
        // Translate
        _rigidbody2D.AddForce(_moveInput *  Time.deltaTime * MoveSpeed);

        // Rotate
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, _lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    public void Look(CallbackContext context)
    {
        if (_isGamepad)
        {
            Vector2 dir = context.action.ReadValue<Vector2>();
            if (dir == Vector2.zero) return;
            _lookDirection = dir.normalized;
        }
        else
        {
            Vector2 pointerPosition = _mainCamera.ScreenToWorldPoint(context.action.ReadValue<Vector2>());
            Vector2 dir;
            dir.x = pointerPosition.x - transform.position.x;
            dir.y = pointerPosition.y - transform.position.y;
            _lookDirection = dir.normalized;
        }
    }

    public void Move(CallbackContext context)
    {
        _moveInput = context.action.ReadValue<Vector2>();
    }


    public void FirePrimary()
    {
        if (PrimaryWeapon == null) return;
        
        if (!_spaceship.CanGlobalFire) return;
        if (!_spaceship.CanPrimaryFire) return;

        if(PrimaryHeat >= PrimaryWeapon.MaximumHeat) return;
        if (_spaceship.IsOverHeating) return;

        if (Input.PrimaryFire.phase == InputActionPhase.Performed)
            _photonView.RPC(Spaceship.RPC_FIRE_PRIMARY, RpcTarget.All, transform.position, transform.rotation, _rigidbody2D.velocity);
    }

    public void FireSpecial()
    {
        if (SpecialWeapon == null) return;

        if (!_spaceship.CanGlobalFire) return;
        if (!_spaceship.CanSpecialFire) return;

        if (SpecialAmmo <= 0) return;
        
        if (Input.SpecialFire.phase == InputActionPhase.Performed)
            _photonView.RPC(Spaceship.RPC_FIRE_SPECIAL, RpcTarget.All, transform.position, transform.rotation, _rigidbody2D.velocity);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        _isGamepad = input.currentControlScheme.Equals("Gamepad");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 velocity = _rigidbody2D.velocity;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Vector3 accel = _acceleration * 5;
        Gizmos.DrawLine(transform.position, transform.position + accel);


    }

  }
