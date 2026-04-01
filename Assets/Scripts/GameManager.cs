using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro _Score1Label;
    [SerializeField] private TextMeshPro _Score2Label;
    [SerializeField] private TextMeshPro _TimeLabel;
    [SerializeField] private GameObject _PlayerSpawner;
    
    [SerializeField] private float _time = 10;

    [SerializeField] private GameObject _EndScreen;
    [SerializeField] private TextMeshProUGUI _EndScore;
    [SerializeField] private TextMeshProUGUI _WinnerText;


    private float _minutes = 0;
    private float _seconds = 0;
    private int _score1 = 0;
    private int _score2 = 0;
    

    public void Player1Scored()
    {
        UpdateScore(_score1, _Score1Label);
    }

    public void Player2Scored()
    {
        UpdateScore(_score2, _Score2Label);
    }

    public void UpdateScore(int score, TextMeshPro scoreLabel)
    {
        score++;

        string scoreString = score.ToString();

        if(score < 10) scoreString = "0" + scoreString;

        scoreLabel.text = scoreString;
    }

    private void Update()
    {
        if (_PlayerSpawner.GetComponent<PlayerSpawner>().GetPlayerCount() < 2) return;
        if (_time >= 1) 
        {
            _time -= Time.deltaTime;
            _minutes = Mathf.Floor(_time / 60);
            _seconds = Mathf.Floor(_time % 60);

            if (_seconds < 10)
            {
                _TimeLabel.text = $"{_minutes}:0{_seconds}";
            }
            else
            {
                _TimeLabel.text = $"{_minutes}:{_seconds}";
            }
        }
        else if(_EndScreen.activeSelf == false)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach(var player in players)
            {
                player.GetComponentInChildren<PlayerInput>().enabled = false;
            }

            _PlayerSpawner.GetComponent<PlayerInputManager>().enabled = false;

            _EndScreen.SetActive(true);

            if(_score1 < _score2)
            {
                _WinnerText.text = "Red Player Wins!";
            }
            else if (_score2 > _score1)
            {
                _WinnerText.text = "Blue Player Wins!";
            }
            else
            {
                _WinnerText.text = "It's a Tie!";
            }

            _EndScore.text = $"{_score1} - {_score2}";
        }
        else
        {
            if(Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
