using TMPro;
using UnityEngine;

public class DisplayScores : MonoBehaviour
{
    [SerializeField] GameObject _ScoreLabel;

    private int score1 = 0;
    private int score2 = 0;

    public void Player1Scored()
    {
        score1++;
        UpdateScore();
    }

    public void Player2Scored()
    {
        score2++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _ScoreLabel.GetComponent<TextMeshProUGUI>().text = score1 + " - " + score2;
    }
}
