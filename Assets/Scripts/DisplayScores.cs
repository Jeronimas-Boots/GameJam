using TMPro;
using UnityEngine;

public class DisplayScores : MonoBehaviour
{
    [SerializeField] TextMeshPro _HomeScore;
    [SerializeField] TextMeshPro _GuestScore;

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
        string score1String = score1.ToString();
        string score2String = score2.ToString();

        if (score1 < 10) score1String = "0" + score1String;
        if (score2 < 10) score2String = "0" + score2String;

        _HomeScore.text = score1String;
        _GuestScore.text = score2String;
    }
}
