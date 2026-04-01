using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public class ObjectSlot
{
    public GameObject gameObject;
    public Transform transform;
    [HideInInspector] public float currentGrabTime = 0f;
}
public class CharacterController : MonoBehaviour
{
    private Rigidbody _rb;

    private RagdollBehaviour ragdollBehaviour;
    public Vector3 movementDirection = Vector3.zero;
    public Quaternion lookAtDirection = Quaternion.identity;
    

    [Range(.5f, 200.0f)] public float movementSpeed;
    [Range(.5f, 400.0f)] public float maxSpeed;
    [Range(.5f, 10000.0f)] public float jumpForce;



    [Header("Grabbing")]
    public float grabbingRange = 1.0f;
    public LayerMask grabbableLayerMask;
    public float ThrowingForce = 10.0f;
    public float maxGrabTime = 5.0f;

    public InputActionReference leftGrab;

    public List<ObjectSlot> slots;

    [Header("Dashing")]

    public float DashForce = 100.0f;
    public float TorqueForce = 360.0f;
    public float DashCooldown = 1.0f;
    public float DashDuration = 1.0f;
    private float DashCooldownTimer = 0.0f;
    private bool CanDash = false;

    public float DashVolume = 1.0f;
    public AudioClip DashSound;

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

    private void Awake()
    {
        ragdollBehaviour = gameObject.GetComponent<RagdollBehaviour>();
        _fallEffect = transform.GetComponentInChildren<ParticleSystem>();
        _fallEffect.transform.SetParent(null, true);
        _field = GameObject.FindAnyObjectByType<Field>();
        if (!(_rb = transform.GetComponent<Rigidbody>()))
            _rb = transform.AddComponent<Rigidbody>();

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.freezeRotation = false;


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
        DashCooldownTimer += Time.deltaTime;
        if(DashCooldownTimer > DashCooldown)
        {
            DashCooldownTimer = 0;
            CanDash = true;
        }
        if (isGrounded)
        {
            ragdollBehaviour.ChangeRagdollMode(0);
        }

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

        // Handle Grab Timers
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject != null)
            {
                slots[i].currentGrabTime += Time.deltaTime;
                if (slots[i].currentGrabTime >= maxGrabTime)
                {
                    ThrowObject(i);
                }
            }
            else
            {
                slots[i].currentGrabTime = 0f;
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
    public void OnDash(InputAction.CallbackContext context)
    {
        if (CanDash)
        {
            CanDash = false;
            animator.enabled = false;
            _rb.AddForce(movementDirection * DashForce,ForceMode.VelocityChange);
            SoundFXManager.Instance.PlaySoundFXClip(DashSound, transform, DashVolume);
            if (ragdollBehaviour)
            {
                ragdollBehaviour.ChangeRagdollMode(1);
            }
            _rb.AddRelativeTorque(new Vector3(
                Random.Range(-1, 1),
                Random.Range(-1, 1),
                Random.Range(-1, 1)
                ).normalized * TorqueForce, ForceMode.VelocityChange);
            //_rb.AddRelativeTorque(transform.right* TorqueForce, ForceMode.VelocityChange);
            Invoke(nameof(ToggleRagdoll),DashDuration);
        }
    }
    public void ToggleRagdoll()
    {
        animator.enabled = true;
        ragdollBehaviour.ChangeRagdollMode(0);
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
        //lookAtDirection = transform.rotation * Quaternion.Euler(0, input.x, 0);
    }
    public void OnGrabObject(InputAction.CallbackContext context)
    {
        
        int index = context.action.name == leftGrab.action.name ? 1 : 0;

        if (index > slots.Count) return;

        if (context.started)
        {
            if (slots[index].gameObject == null)
            {
                GrabNearestRigidBodyObject(index);
            }
        }
        
        if(context.canceled)
        {
            if(slots[index].gameObject != null)
            {
                ThrowObject(index);
            }
        }
    }

    private void ThrowObject(int handIndex)
    {
        var obj = slots[handIndex].gameObject;
        if (obj == null) return;

        var rb = obj.GetComponent<Rigidbody>();
        var cl = obj.GetComponent<Collider>();

        var joint = slots[handIndex].transform.GetComponent<Joint>();
        if (joint != null)
        {
            Destroy(joint);
        }
        else
        {
            obj.transform.SetParent(null, true);
            if (cl != null) cl.enabled = true;
            if (rb != null) rb.isKinematic = false;
        }

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
        slots[handIndex].currentGrabTime = 0f; // Reset grab timer
    }

    // Helper method to check if THIS character is currently holding another player
    public bool IsGrabbingAnotherPlayer()
    {
        foreach (var slot in slots)
        {
            if (slot.gameObject != null)
            {
                CharacterController cc = slot.gameObject.transform.root.GetComponent<CharacterController>();
                if (cc != null && cc != this)
                {
                    return true;
                }
            }
        }
        return false;
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

            // If the hit object is a player, check if they are currently busy grabbing someone
            CharacterController hitCharacter = hit.transform.root.GetComponent<CharacterController>();
            if (hitCharacter != null && hitCharacter.IsGrabbingAnotherPlayer())
            {
                continue; // Cannot grab this player because they are currently grabbing someone else
            }

            var rb = hit.gameObject.GetComponent<Rigidbody>();
            var cl = hit.gameObject.GetComponent<Collider>();
            if (rb != null && cl != null)
            {
                slots[handIndex].gameObject = hit.gameObject;
                slots[handIndex].currentGrabTime = 0f; // Start the timer

                if (hitCharacter != null)
                {
                    Rigidbody handRb = slots[handIndex].transform.GetComponent<Rigidbody>();
                    if (handRb == null)
                    {
                        handRb = slots[handIndex].transform.gameObject.AddComponent<Rigidbody>();
                        handRb.isKinematic = true;
                    }

                    ConfigurableJoint joint = slots[handIndex].transform.gameObject.AddComponent<ConfigurableJoint>();
                    joint.connectedBody = rb;

                    joint.autoConfigureConnectedAnchor = true;

                    joint.xMotion = ConfigurableJointMotion.Locked;
                    joint.yMotion = ConfigurableJointMotion.Locked;
                    joint.zMotion = ConfigurableJointMotion.Locked;

                    joint.angularXMotion = ConfigurableJointMotion.Free;
                    joint.angularYMotion = ConfigurableJointMotion.Free;
                    joint.angularZMotion = ConfigurableJointMotion.Free;
                }
                else
                {
                    hit.gameObject.transform.position = slots[handIndex].transform.position;
                    hit.gameObject.transform.SetParent(slots[handIndex].transform, true);
                    cl.enabled = false;
                    rb.isKinematic = true;
                }

                return;
            }
        }
    }
}
