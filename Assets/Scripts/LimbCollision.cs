using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    public CharacterController characterController;
    void Start()
    {
        characterController = FindAnyObjectByType<CharacterController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (characterController != null)
        {
            characterController.isGrounded = true;
        }

    }
}
