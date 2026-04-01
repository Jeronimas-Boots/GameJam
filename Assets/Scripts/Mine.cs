using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class Mine : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private AudioClip goalSound;

    [SerializeField] public float ExplosionVolume = 1.0f;
    [SerializeField] public float ExplosionForce = 100.0f;
    [SerializeField] public float ExplosionRadius = 100.0f;

    public List<string> TagsToIgnore;

    private bool isActive = false;
    public GameObject myOwner;
    public void SetOwner(GameObject owner)
    {
        myOwner = owner;
    }
    public void SetMineActive()
    {
        Debug.Log("IsActive");
        isActive = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f,0.0f, 0.0f,0.3f);
        Gizmos.DrawSphere(transform.position,ExplosionRadius);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!TagsToIgnore.Contains(other.tag))
        {
            if (other.gameObject.transform.root.gameObject != myOwner)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        SoundFXManager.Instance.PlaySoundFXClip(goalSound, transform, ExplosionVolume);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        foreach (var obj in FindObjectsByType<Rigidbody>())
        {
            obj.AddExplosionForce(ExplosionForce, gameObject.transform.position, ExplosionRadius);
        }
        ParticleSystem ps = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(ps.gameObject, ps.main.duration);
        Destroy(gameObject);
    }
}
