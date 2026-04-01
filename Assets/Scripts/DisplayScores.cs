using TMPro;
using UnityEngine;

public class DisplayScores : MonoBehaviour
{
    [SerializeField] private TextMeshPro _Score1Label;
    [SerializeField] private TextMeshPro _Score2Label;
    [SerializeField] private TextMeshPro _TimeLabel;
    [SerializeField] private PlayerSpawner _PlayerSpawner;
    [SerializeField] private float _time = 300;


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
        if (_time > 0 && _PlayerSpawner.GetPlayerCount() == 2) 
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
    }
}
