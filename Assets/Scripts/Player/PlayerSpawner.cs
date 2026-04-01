using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] _transforms;
    [SerializeField] private Color[] _colors;
    private int _playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (_playerCount < _transforms.Length)
        {
            playerInput.GetComponent<CharacterController>().InnitializePlayer(_transforms[_playerCount], _colors[_playerCount]);
            _playerCount++;
        }
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }
}
