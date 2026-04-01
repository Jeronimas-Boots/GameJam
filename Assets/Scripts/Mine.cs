using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] public float ExplosionDuration = 1.0f;
    [SerializeField] public float ExplosionForce = 100.0f;
    [SerializeField] public float ExplosionRadius = 100.0f;

    private bool isActive = false;
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

    private void Explode()
    {
        //Collider[] hits =  Physics.OverlapSphere(transform.position , ExplosionRadius);
        //foreach (var hit in hits) {
        //    var rb = hit.gameObject.GetComponent<Rigidbody>();
        //    if (rb) { 
        //        rb.AddExplosionForce(ExplosionForce, new Vector3(), ExplosionRadius);
        //    }
        //}
        foreach (var obj in FindObjectsByType<Rigidbody>())
        {
            obj.AddExplosionForce(ExplosionForce, gameObject.transform.position, ExplosionRadius);
        }
        Invoke(nameof(DestroySelf), ExplosionDuration);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
