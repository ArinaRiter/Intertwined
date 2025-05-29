using System;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private ScoreView scoreView;
    
    private void Start()
    {
        //ScoreModel.ResetScore();
    }
    

    public void AddScore(int score)
    {
        ScoreModel.AddPoints(score);
        scoreView.UpdateScore(score);
    }
    
}
