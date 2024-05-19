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




    private InputSystem _inputSystem;
    private Vector2 _moveInput;
    private bool isGamepad = true;

    private InputSystem.PlayerActions Input { get { return _inputSystem.Player; } }

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
        LookAtCrosshair();
        MoveCrosshair();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }



    private void Movement()
    {
        _rigidbody2D.AddForce(_moveInput *  Time.deltaTime * MoveSpeed);
    }

    private void LookAtCrosshair()
    {
        Vector3 direction;

        if (isGamepad)
        {
            if (Input.Crosshair.ReadValue<Vector2>() == Vector2.zero) return;
            direction = Input.Crosshair.ReadValue<Vector2>().normalized;
        }
        else
        {
            direction = (_crossHair.position - transform.position).normalized;
          
        }

        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 velocity = _rigidbody2D.velocity;

        Debug.Log(velocity);

        Gizmos.DrawLine(transform.position, transform.position + velocity);

    }

}
