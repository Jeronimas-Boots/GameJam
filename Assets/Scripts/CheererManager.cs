using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class CheererManager : MonoBehaviour
{
    private float _cheererSpeedup = -1f;
    private List<Cheerie> _cheerers = new List<Cheerie>();
    void Start()
    {
        _cheerers = FindObjectsByType<Cheerie>().ToList();
        foreach (var cheerie in _cheerers)
        {
            cheerie.upDownAmountMultiplier *= Random.Range(0.5f, 1.5f);
            cheerie.speedMultiplier *= Random.Range(0.8f, 1.2f);
            cheerie.sinUpDownAmount = Random.Range(0f, 100f);
        }
    }

    private void Update()
    {
        if (_cheererSpeedup > 0f)
        {
            _cheererSpeedup -= Time.deltaTime;
            if (_cheererSpeedup < 0f)
            {
                _cheererSpeedup = -1f;
                foreach (var cheerie in _cheerers)
                {
                    cheerie.speedMultiplier /= 2;
                }
            }
        }
    }

    public void GoalOccurred()
    {
        _cheererSpeedup = 3f;
        foreach (var cheerie in _cheerers)
        {
            cheerie.speedMultiplier *= 2;
        }
    }
}
