using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] _transforms;
    [SerializeField] private Color[] _colors;
    [SerializeField] private GameObject _ball;
    private int _playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (_playerCount < _transforms.Length)
        {
            playerInput.GetComponent<CharacterController>().InnitializePlayer(_transforms[_playerCount], _colors[_playerCount]);
            _playerCount++;
        }

        if(_playerCount == 2)
        {
            Instantiate(_ball, new Vector3(0, 2, 0), new Quaternion());
        }
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }
}
