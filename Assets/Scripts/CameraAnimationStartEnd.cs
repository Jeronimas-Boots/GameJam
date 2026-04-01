using System;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraAnimationStartEnd : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxUpRotation;
    [SerializeField] private float _maxDownRotation;
    private bool _move = false;
    private bool _moveUp = false;
    public void StartMove(bool moveUp)
    {
        _move = true;
        _moveUp = moveUp;
    }

    private void Start()
    {
        _maxDownRotation = -_maxDownRotation;
        StartMove(true);
    }
    private float testTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        testTimer += Time.deltaTime;
        if (testTimer > 10f)
        {
            // Test flip
            StartMove(!_moveUp);
            testTimer = 0f;
        }

        if (_move)
        {
            Vector3 deltaMove;
            if (_moveUp)
                deltaMove = new Vector3(-1f, 0, 0);
            else
                deltaMove = new Vector3(1f,0,0);

            transform.eulerAngles += deltaMove * _speed * Time.deltaTime;

            if (_moveUp)
            {
                if (transform.eulerAngles.x > 180 && (transform.eulerAngles.x - (360 - _maxDownRotation)) < 0.2f)
                {
                    _move = false;
                    transform.eulerAngles = new Vector3(-_maxDownRotation, 0, 0);
                }
            }
            else
            {

                if (transform.eulerAngles.x > _maxUpRotation && !(transform.eulerAngles.x > 180))
                {
                    _move = false;
                    transform.eulerAngles = new Vector3(_maxUpRotation, 0, 0);
                }
            }
        }
    }
}
