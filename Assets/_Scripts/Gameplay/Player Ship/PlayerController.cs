using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Camera _mainCamera;
    [SerializeField] ShipController _shipController;

    [field: Header("Unit Variables")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1000;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 10;
    [field: SerializeField] public float LookSpeed { get; private set; } = 10;



    private bool _canFire = true; 


    public Vector2 LookDirection { get; private set; }

    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private bool _isGamepad = true;

    private InputSystem.PlayerActions Input { get { return _inputSystem.Player; } }


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

    private void Update()
    {
        Look();
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
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, LookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    private void Look()
    {
        if (_isGamepad)
        {
            if (Input.Look.ReadValue<Vector2>() == Vector2.zero) return;
            LookDirection = Input.Look.ReadValue<Vector2>().normalized;
        }
        else
        {
            Vector2 pointerPosition = _mainCamera.ScreenToWorldPoint(Input.Pointer.ReadValue<Vector2>());
            Vector2 dir;
            dir.x = pointerPosition.x - transform.position.x;
            dir.y = pointerPosition.y - transform.position.y;
            LookDirection = dir.normalized;
        }
    }


    public void OnMove(CallbackContext context)
    {
        _moveInput = context.action.ReadValue<Vector2>();
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
