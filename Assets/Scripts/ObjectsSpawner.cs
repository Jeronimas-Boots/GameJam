using System.Linq;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _itemsToSpawn;
    [SerializeField]
    private float _spawnDelay = 1f;
    [SerializeField]
    private float _spawnDelayVariance = 0.5f;

    private float _spawnTimer = 0f;
    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0f)
        {
            _spawnTimer = _spawnDelay + Random.Range(_spawnDelayVariance, -_spawnDelayVariance);
            Instantiate(_itemsToSpawn[Random.Range(0, _itemsToSpawn.Count() - 1)], new Vector3(0,3,0), new Quaternion(), transform ); 
        }
    }
}
