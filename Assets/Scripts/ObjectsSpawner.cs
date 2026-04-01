using System.Linq;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _itemsToSpawn;
    [SerializeField]
    private float _maxItems = 5f;
    [SerializeField]
    private float _spawnDelay = 1f;
    [SerializeField]
    private float _spawnDelayVariance = 0.5f;

    private float _spawnTimer = 0f;

    private Vector3 _startSpawnPos;
    private Vector3 _endSpawnPos;
    private void Start()
    {
        if (transform.childCount > 2) return;

        // Get their positions and save them
        _startSpawnPos = transform.GetChild(0).position;
        _endSpawnPos = transform.GetChild(1).position;

        // Remove them
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(0).gameObject);
    }
    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0f && transform.childCount < _maxItems)
        {
            _spawnTimer = _spawnDelay + Random.Range(_spawnDelayVariance, -_spawnDelayVariance);
            Instantiate(_itemsToSpawn[Random.Range(0, _itemsToSpawn.Length - 1)], new Vector3(Random.Range(_startSpawnPos.x, _endSpawnPos.x), Random.Range(_startSpawnPos.y, _endSpawnPos.y), Random.Range(_startSpawnPos.z, _endSpawnPos.z)), new Quaternion(), transform ); 
        }
    }
}
