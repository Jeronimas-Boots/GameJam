using UnityEngine;

public class Mine : MonoBehaviour
{

    [SerializeField] public float ExplosionForce = 100.0f;
    [SerializeField] public float ExplosionRadius = 100.0f;

    private bool isActive;
    public void SetMineActive()
    {
        isActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player") || other.CompareTag("Ball"))
        {
            other.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
        }
    }
}
