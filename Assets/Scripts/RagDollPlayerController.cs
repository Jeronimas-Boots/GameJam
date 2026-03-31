using UnityEngine;

public class RagDollPlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float strafeSpeed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody hips;
    [SerializeField]
    private bool isGrounded;



    void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                hips.AddForce(hips.transform.forward * speed * 1.5f) ;
            }
            else
            {
                hips.AddForce(hips.transform.forward * speed);
            }
        }

    }
}
