using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 m_movementDirection = Vector3.zero;
    public Quaternion m_lookAtDirection = Quaternion.identity;

    [Range(.5f, 100.0f)] public float movementSpeed;
    [Range(.5f, 200.0f)] public float maxSpeed;
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
    public void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        m_movementDirection = new Vector3(input.x ,0, input.y);
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        m_lookAtDirection = Quaternion.LookRotation(new Vector3(input.x, 0, input.y),Vector3.up);

    }
    public void OnThrowProjectile(InputAction.CallbackContext context)
    {
        Debug.Log("Throwing");

    }
}
