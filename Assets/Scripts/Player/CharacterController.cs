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

    [Range(.5f, 200.0f)] public float movementSpeed;
    [Range(.5f, 400.0f)] public float maxSpeed;
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
        if (!(_rb = transform.GetComponent<Rigidbody>()))
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
                if (_field.Explode(transform.position, 1f))
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
        movementDirection = new Vector3(input.x, 0, input.y);

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

        if (index < slots.Count)
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
        var rb = obj.GetComponent<Rigidbody>();
        var cl = obj.GetComponent<Collider>();

        // Use the base 'Joint' class so it easily catches FixedJoint, ConfigurableJoint, etc.
        var joint = slots[handIndex].transform.GetComponent<Joint>();
        if (joint != null)
        {
            Destroy(joint);
        }
        else
        {
            // It was a prop, undo the parenting and kinematic state
            obj.transform.SetParent(null, true);
            if (cl != null) cl.enabled = true;
            if (rb != null) rb.isKinematic = false;
        }

        // Check if its a mine then active it
        var mine = obj.GetComponent<Mine>();
        if (mine)
        {
            mine.SetMineActive();
            mine.SetOwner(transform.root.gameObject);
            if (cl != null) cl.isTrigger = true;
        }

        if (rb != null)
        {
            rb.AddForce(gameObject.transform.forward * ThrowingForce, ForceMode.Force);
        }

        slots[handIndex].gameObject = null;
    }

    private void GrabNearestRigidBodyObject(int handIndex)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, grabbingRange, grabbableLayerMask);
        foreach (var hit in hits)
        {
            if (hit.transform.root == transform.root)
            {
                continue;
            }

            var rb = hit.gameObject.GetComponent<Rigidbody>();
            var cl = hit.gameObject.GetComponent<Collider>();
            if (rb != null && cl != null)
            {
                slots[handIndex].gameObject = hit.gameObject;

                // If the root has a CharacterController, treat it as a ragdoll/character
                if (hit.transform.root.GetComponent<CharacterController>() != null)
                {
                    Rigidbody handRb = slots[handIndex].transform.GetComponent<Rigidbody>();
                    if (handRb == null)
                    {
                        handRb = slots[handIndex].transform.gameObject.AddComponent<Rigidbody>();
                        handRb.isKinematic = true;
                    }

                    // DO NOT change the hit.gameObject.transform.position here!
                    // Let the joint connect at the current distance to avoid collider overlap.

                    ConfigurableJoint joint = slots[handIndex].transform.gameObject.AddComponent<ConfigurableJoint>();
                    joint.connectedBody = rb;

                    // This tells the joint to maintain the distance between the hand and the body part
                    joint.autoConfigureConnectedAnchor = true;

                    // Lock position (keeps them at arm's length)
                    joint.xMotion = ConfigurableJointMotion.Locked;
                    joint.yMotion = ConfigurableJointMotion.Locked;
                    joint.zMotion = ConfigurableJointMotion.Locked;

                    // Free rotation (allow them to stay upright)
                    joint.angularXMotion = ConfigurableJointMotion.Free;
                    joint.angularYMotion = ConfigurableJointMotion.Free;
                    joint.angularZMotion = ConfigurableJointMotion.Free;
                }
                else
                {
                    // Otherwise, treat it as a prop/mine
                    hit.gameObject.transform.position = slots[handIndex].transform.position; // Only teleport props
                    hit.gameObject.transform.SetParent(slots[handIndex].transform, true);
                    cl.enabled = false;
                    rb.isKinematic = true;
                }

                return;
            }
        }
    }
}
