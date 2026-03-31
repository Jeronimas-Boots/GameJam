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
    private Rigidbody rb;

    [Header("Movement")]
    public Vector3 m_movementDirection = Vector3.zero;
    public Quaternion m_lookAtDirection = Quaternion.identity;

    [Range(.5f, 100.0f)] public float movementSpeed;
    [Range(.5f, 200.0f)] public float maxSpeed;
    [Range(.5f, 10000.0f)] public float jumpForce;



    [Header("Grabbing")]
    public float grabbingRange = 1.0f;
    public LayerMask grabbableLayerMask;
    public float ThrowingForce = 10.0f;

    public InputActionReference leftGrab; 

    public List<ObjectSlot> slots;

    private void Start()
    {
        if(!(rb = transform.GetComponent<Rigidbody>()))
            rb = transform.AddComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;
    }
    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(m_movementDirection * movementSpeed, ForceMode.Acceleration);
            Vector3 vel = rb.linearVelocity;
            Vector3 horizontal = new Vector3(vel.x, 0, vel.z);

            if (horizontal.magnitude > maxSpeed)
            {
                horizontal = horizontal.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(horizontal.x, vel.y, horizontal.z);
            }
        }
    }
    private void Update()
    {
        if (rb != null) {
            rb.transform.rotation = m_lookAtDirection;
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
        m_movementDirection = new Vector3(input.x ,0, input.y);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        m_lookAtDirection = Quaternion.LookRotation(new Vector3(input.x, 0, input.y),Vector3.up);

    }
    public void OnGrabObject(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        int index = context.action.name == leftGrab.action.name ? 0 : 1;

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
