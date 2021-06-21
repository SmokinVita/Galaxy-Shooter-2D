using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isGameOver = false;

    private bool _isCheckingForEnemy = false;
    GameObject[] _aliveEnemies;
    [SerializeField]
    private GameObject _asteroid;
    [SerializeField]
    private SpawnManager _spawnManager;

    public void StartCheckingForEnemies()
    {
        _isCheckingForEnemy = true;
        StartCoroutine(CheckForEnemiesRoutine());
    }

    private IEnumerator CheckForEnemiesRoutine()
    {
        while (_isCheckingForEnemy)
        {
            yield return new WaitForSeconds(2f);
            _aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            if(_aliveEnemies.Length <= 0)
            {
                Debug.Log("No enemies!");
                _spawnManager.StopSpawning();
                Instantiate(_asteroid, transform.position, Quaternion.identity);
                _isCheckingForEnemy = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(1); //1 is the Game Scene, 0 is Main menu

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
