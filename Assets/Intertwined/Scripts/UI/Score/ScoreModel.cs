using System;
using UnityEngine;

public static class ScoreModel
{
    public static int Score { get; private set; }
    
    public static event Action<int> OnScoreChanged;

    public static void AddPoints(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    public static void SubtractPoints(int points)
    {
        Score -= points;
        OnScoreChanged?.Invoke(Score);
    }

    public static void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }
}
