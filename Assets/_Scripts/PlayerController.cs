using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{


    [Header("Components")]
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Transform _crossHair;
    [SerializeField] Camera _mainCamera;

    [field: Header("Unit Variables")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 5;
    [field: SerializeField] public float LookSpeed { get; private set; } = 5;
    [field: SerializeField] public float DashStrength { get; private set; } = 5;

    


    public Vector2 LookDirection { get; private set; }

    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private bool isGamepad = true;

    private InputSystem.PlayerActions Input { get { return _inputSystem.Player; } }


    private Vector2 _acceleration = Vector3.zero;
    private Vector2 _prevVelocity = Vector3.zero;


    private void Awake()
    {
        _inputSystem = new InputSystem();
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
        _moveInput = Input.Move.ReadValue<Vector2>();
        Look();
        MoveCrosshair();


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
        _rigidbody2D.AddForce(_moveInput *  Time.deltaTime * MoveSpeed);
    }

    private void Look()
    {

        if (isGamepad)
        {
            if (Input.Look.ReadValue<Vector2>() == Vector2.zero) return;
            LookDirection = Input.Look.ReadValue<Vector2>().normalized;
        }
        else
        {
            LookDirection = (_crossHair.position - transform.position).normalized;
          
        }

        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, LookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
    }

    private void MoveCrosshair()
    {
        if (isGamepad)
        {

            _crossHair.position = transform.position + (transform.up * 2);
        }
        else
        {
            Vector3 position = _mainCamera.ScreenToWorldPoint(Input.Pointer.ReadValue<Vector2>());
            position.z = _crossHair.position.z;
            _crossHair.position = position;
        }
        
    }

    public void OnControlsChanged(PlayerInput input)
    {
        isGamepad = input.currentControlScheme.Equals("Gamepad");
    }

    public void Dash()
    {
        _rigidbody2D.AddForce(transform.up * DashStrength, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 velocity = _rigidbody2D.velocity;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Vector3 accel = _acceleration * 5;
        Debug.Log(accel);
        Gizmos.DrawLine(transform.position, transform.position + accel);


    }

}
