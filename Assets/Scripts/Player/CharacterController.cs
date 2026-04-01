using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ObjectSlot
{
    public GameObject gameObject;
    public Transform transform;
}
public class CharacterController : MonoBehaviour
{
    private Rigidbody _rb;

    public Vector3 movementDirection = Vector3.zero;
    public Quaternion lookAtDirection = Quaternion.identity;

    [Range(.5f, 100.0f)] public float movementSpeed;
    [Range(.5f, 200.0f)] public float maxSpeed;
    [Range(.5f, 10000.0f)] public float jumpForce;



    [Header("Grabbing")]
    public float grabbingRange = 1.0f;
    public LayerMask grabbableLayerMask;
    public float ThrowingForce = 10.0f;

    public InputActionReference leftGrab; 

    public List<ObjectSlot> slots;

    [HideInInspector]
    public bool isGrounded;

    [SerializeField]
    private Animator animator;


    private Field _field;
    private bool _justJumped;
    private float _jumpedTimeAgo = 0f;
    private ParticleSystem _fallEffect;
    private Vector3 _lastFallPosition;

    [SerializeField] private GameObject _mainBody;

    private void Start()
    {
        _fallEffect = transform.GetComponentInChildren<ParticleSystem>();
        _fallEffect.transform.SetParent(null, true);
        _field = GameObject.FindAnyObjectByType<Field>();
        if(!(_rb = transform.GetComponent<Rigidbody>()))
            _rb = transform.AddComponent<Rigidbody>();

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.freezeRotation = true;
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

            if (_rb != null)
            {
                _rb.MoveRotation(lookAtDirection);
            }
        }
    }
    private void Update()
    {
        if (_justJumped && _field != null && _fallEffect != null)
        {
            _jumpedTimeAgo += Time.deltaTime;
            if (_jumpedTimeAgo > 0.2f && isGrounded)
            {
                _justJumped = false;
                if (_field.Explode(transform.position, 1.5f))
                {
                    _fallEffect.transform.position = transform.position;
                    _fallEffect.Play();
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, .3f);
        Gizmos.DrawSphere(transform.position, grabbingRange);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        movementDirection = new Vector3(input.x ,0, input.y);

        bool isWalking = input.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);

        // Update look direction to match movement direction if currently moving
        if (isWalking)
        {
            lookAtDirection = Quaternion.LookRotation(movementDirection, Vector3.up);
        }
    }

    public void InnitializePlayer(Transform startTransform, Color color)
    {
        _mainBody.transform.position = startTransform.position;
        _mainBody.transform.rotation = startTransform.rotation;

        _mainBody.GetComponentInChildren<SkinnedMeshRenderer>().material.color = color;
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
        lookAtDirection = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);

    }
    public void OnGrabObject(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        int index = context.action.name == leftGrab.action.name ? 1 : 0;

        if(index < slots.Count)
        {
            if (slots[index].gameObject == null)
            {
                GrabNearestRigidBodyObject(index);
            }
            else
            {
                ThrowObject(index);
            }
        }
    }

    private void ThrowObject(int handIndex) 
    {
        var obj = slots[handIndex].gameObject;

        //Check if its a mine then active it or something
        var mine = gameObject.GetComponent<Mine>();
        if (mine)
        {
            mine.SetMineActive();
        }


        obj.transform.SetParent(null, true);
        var rb = obj.gameObject.GetComponent<Rigidbody>();
        var cl = obj.gameObject.GetComponent<Collider>();

        cl.enabled = true;
        rb.isKinematic = false;
        rb.AddForce(gameObject.transform.forward * ThrowingForce, ForceMode.Force);
        slots[handIndex].gameObject = null;

    }
    private void GrabNearestRigidBodyObject(int handIndex)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, grabbingRange, grabbableLayerMask);
        foreach (var hit in hits)
        {
            var rb = hit.gameObject.GetComponent<Rigidbody>();
            var cl = hit.gameObject.GetComponent<Collider>();
            if (rb != null && cl != null)
            {
                hit.gameObject.transform.position = slots[handIndex].transform.position;
                hit.gameObject.transform.SetParent(slots[handIndex].transform, true);
                slots[handIndex].gameObject = hit.gameObject;
                cl.enabled = false;
                rb.isKinematic = true;
                return;
            }
        }
    }
}
