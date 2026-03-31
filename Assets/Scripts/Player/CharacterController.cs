using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour
{
    private Rigidbody _rb;

    public Vector3 movementDirection = Vector3.zero;
    public Quaternion lookAtDirection = Quaternion.identity;

    [Range(.5f, 100.0f)] public float movementSpeed;
    [Range(.5f, 200.0f)] public float maxSpeed;
    [Range(.5f, 10000.0f)] public float jumpForce;

    [HideInInspector]
    public bool isGrounded;

    [SerializeField]
    private Animator animator;


    private Field _field;
    private bool _justJumped;
    private float _jumpedTimeAgo = 0f;
    private void Start()
    {
        _field = GameObject.FindAnyObjectByType<Field>();
        if(!(_rb = transform.GetComponent<Rigidbody>()))
            _rb = transform.AddComponent<Rigidbody>();

        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
    private void FixedUpdate()
    {
        if (_rb != null)
        {
            _rb.AddForce(movementDirection * movementSpeed, ForceMode.Acceleration);
            Vector3 vel = _rb.linearVelocity;
            Vector3 horizontal = new Vector3(vel.x, 0, vel.z);

            if (horizontal.magnitude > maxSpeed)
            {
                horizontal = horizontal.normalized * maxSpeed;
                _rb.linearVelocity = new Vector3(horizontal.x, vel.y, horizontal.z);
            }
        }
    }
    private void Update()
    {
        if (_justJumped)
        {
            _jumpedTimeAgo += Time.deltaTime;
            if (_jumpedTimeAgo > 0.2f && isGrounded)
            {
                _justJumped = false;
                _field.Explode(transform.position, 1.5f);
            }
        }
        if (_rb != null) {
            _rb.transform.rotation = lookAtDirection;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        movementDirection = new Vector3(input.x ,0, input.y);

        bool isWalking = input.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
            isGrounded = false;
            _justJumped = true;
            _jumpedTimeAgo = 0f;
        }
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        lookAtDirection = Quaternion.LookRotation(new Vector3(input.x, 0, input.y),Vector3.up);

    }
    public void OnThrowProjectile(InputAction.CallbackContext context)
    {
        Debug.Log("Throwing");

    }
}
