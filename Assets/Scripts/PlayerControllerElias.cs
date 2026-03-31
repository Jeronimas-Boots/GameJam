using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private UnityEngine.CharacterController _controller;
    [SerializeField] private float _fallingspeed = 3f;

    private Vector2 _direction = Vector3.zero;
    private float _verticalVelocity = 0f;
    private float _gravity = 9.81f;

    private void Update()
    {
        if(_controller.enabled)
        {
            Vector3 moveVelocity = new Vector3(_direction.x, 0f, _direction.y) * _moveSpeed;

            //Apply gravity to player
            if (_controller.isGrounded && _verticalVelocity < 0)
            {
                _verticalVelocity -= _fallingspeed;
            }
            else
            {
                _verticalVelocity -= _gravity * Time.deltaTime;
            }

            moveVelocity.y = _verticalVelocity;

            _controller.Move(moveVelocity * Time.deltaTime);
        }
        else
        {
            if(transform.position.y <= 1f)
            {
                _controller.enabled = true;
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }
}
