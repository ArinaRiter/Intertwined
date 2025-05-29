using System;
using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
}
