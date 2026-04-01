using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    public CharacterController characterController;
    void Start()
    {
        if (characterController == null)
        {
            characterController = transform.root.GetComponentInChildren<CharacterController>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (characterController != null && collision.transform.root != transform.root)
        {

            characterController.isGrounded = true;
        }

    }
}
