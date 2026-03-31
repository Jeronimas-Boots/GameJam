using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterController>().enabled = false;
        
        other.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, 5f);
    }
}
