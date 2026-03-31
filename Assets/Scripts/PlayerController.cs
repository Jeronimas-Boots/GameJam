using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private CharacterController _controller;

    private Vector2 _direction = Vector3.zero;

    private void Update()
    {
        if(_controller.enabled)
        {
            Vector3 moveVelocity = new Vector3(_direction.x, 0f, _direction.y) * _moveSpeed;
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
