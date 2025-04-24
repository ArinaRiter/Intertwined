using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    private EntityStats _entityStats;
    void Awake()
    {
        _entityStats = FindFirstObjectByType<PlayerController>().GetComponent<EntityStats>();
    }

    private void OnEnable()
    {
        _entityStats.OnDeath += OnDie;
    }

    private void OnDisable()
    {
        _entityStats.OnDeath -= OnDie;
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
