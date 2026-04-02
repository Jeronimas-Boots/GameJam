using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PathFollower : MonoBehaviour
{
    public List<GameObject> _waypoints;
    public float _speed = 5f;

    private int _currentWaypointIndex = 0;

    void Update()
    {
        if (_waypoints.Count == 0) return;
        Transform target = _waypoints[_currentWaypointIndex].transform;
        Vector3 direction = (target.position - transform.position).normalized;


        transform.position += direction * _speed * Time.deltaTime;

        // Check if close enough to switch waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Count)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
