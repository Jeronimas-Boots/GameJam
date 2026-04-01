using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private AudioClip goalSound;

    [SerializeField] public float ExplosionVolume = 1.0f;
    [SerializeField] public float ExplosionForce = 100.0f;
    [SerializeField] public float ExplosionRadius = 5.0f;

    [SerializeField] private float TimeToExplode = 10f;
    private float _timer = 0f;
    private float _blinkTimer = 0f;
    [SerializeField] private float _startLerpExplode = 0.01f;
    [SerializeField] private float _endLerpExplode = 0.5f;

    [SerializeField] private Color redColor;
    private Color _currentColor;
    private Renderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _currentColor = _renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer/ TimeToExplode > 0.92f)
        {
            _renderer.material.color = redColor;
        }
        else
        {
            _blinkTimer += (Time.deltaTime + (_timer / TimeToExplode)/2) / 6;
            if (_blinkTimer > 0.5f)
            {
                // Color red
                _renderer.material.color = redColor;

                // If timer over 1, move back by 1
                if (_blinkTimer > 1f)
                {
                    _blinkTimer -= 1f;
                }
            }
            else
            {
                // Color normal
                _renderer.material.color = _currentColor;
            }
        }



        if (_timer > TimeToExplode)
        {
            foreach (var rigidbody in FindObjectsByType<Rigidbody>())
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
            ParticleSystem ps = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            Destroy(ps.gameObject, ps.main.duration);

            SoundFXManager.Instance.PlaySoundFXClip(goalSound, transform, ExplosionVolume);

            FindAnyObjectByType<Field>().Explode(transform.position, ExplosionRadius / 4);
            Destroy(gameObject);
        }
    }
}