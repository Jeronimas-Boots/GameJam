using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private bool _devMode = false;
    [SerializeField] private Transform[] _transforms;
    [SerializeField] private Color[] _colors;
    [SerializeField] private GameObject _ball;
    private int _playerCount = 0;
    private bool _gameStarted = false;
    private GameObject _pressAToJoinUI;

    public void Awake()
    {
        _pressAToJoinUI = GameObject.Find("PressAToJoinUI");
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (!_gameStarted)
        {
            _gameStarted = true;
            FindAnyObjectByType<StartScreen>().ClickStart();
            _pressAToJoinUI.SetActive(true);
        }
        if (GetPlayerCount() < _transforms.Length)
        {
            playerInput.GetComponent<CharacterController>().InnitializePlayer(_transforms[_playerCount], _colors[_playerCount]);
            _playerCount++;
        }

        if(GetPlayerCount() == 2)
        {
            var ball = Instantiate(_ball, new Vector3(0, 2, 0), new Quaternion());
            foreach (Goal goal in FindObjectsByType<Goal>())
            {
                goal.ball = ball;
            }

            _pressAToJoinUI.SetActive(false);

            foreach (var player in FindObjectsByType<CharacterController>())
            {
                player._canMove = true;
            }
        }
    }

    public int GetPlayerCount()
    {
        if (_devMode) return _playerCount + 1;
        return _playerCount;
    }
}
