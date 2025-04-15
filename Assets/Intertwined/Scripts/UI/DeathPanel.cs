using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    private CharacterStats _characterStats;
    void Awake()
    {
        _characterStats = FindFirstObjectByType<PlayerController>().GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        _characterStats.OnDeath += OnDie;
    }

    private void OnDisable()
    {
        _characterStats.OnDeath -= OnDie;
    }

    private void OnDie() 
    {
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine() 
    {
        deathPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
