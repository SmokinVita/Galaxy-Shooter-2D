using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject[] _enemyPrefab;
    private GameObject _selectedEnemy;
    private int _totalEnemyWeight = 0;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;
    private GameObject _selectedPowerup;

    private int _totalPowerupWeight = 0;
    
    private bool _stopSpawning = false;

    [SerializeField]
    private int _maxWave = 3;
    [SerializeField]
    private int _currentWave;

    [Tooltip("The amount of enemies inputed will be multiplied by the current wave.")]
    [SerializeField]
    private int _amountOfEnemies = 3;

    [SerializeField]
    private UIManager _uIManager;

    void Start()
    {
        TotalEnemyWeightAmount();
        TotalPowerupWeightAmount();
    }
    public void StartSpawning()
    {
        StartWave();
        
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    //Wave System 
    // each wave has more enemies than last. 
    //spawn in astroid when all enemies are over
    //Give notification on what wave it is.
    //When max wave is met, spawn Boss. 
    private void StartWave()
    {
        if (_currentWave == _maxWave)
        {
            Debug.Log("Enter Boss!");
            _uIManager.BossIncoming();
        }
        else
        {
            _currentWave++;
            _uIManager.StartOfWave(_currentWave);
            StartCoroutine(SpawnEnemyRoutine(_amountOfEnemies * _currentWave));
        }
        
    }
    

    private void TotalEnemyWeightAmount()
    {
        for (int i = 0; i < _enemyPrefab.Length; i++)
        {
            _totalEnemyWeight += _enemyPrefab[i].GetComponent<Enemy>()._enemyWeight;
        }
    }

    private void PickEnemyToSpawn()
    {
        int randomNumber = UnityEngine.Random.Range(0, _totalEnemyWeight);

        foreach (var enemy in _enemyPrefab)
        {
            int enemyWeight = enemy.GetComponent<Enemy>()._enemyWeight;
            if(randomNumber <= enemyWeight)
            {
                _selectedEnemy = enemy;
                break;
            }
            randomNumber -= enemyWeight;
        }
    }

    IEnumerator SpawnEnemyRoutine(int enemiesToSpawn)
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false && enemiesToSpawn > 0)
        {
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-9, 9), 7, 0);
            PickEnemyToSpawn();
            GameObject newEnemy = Instantiate(_selectedEnemy, spawnArea, transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            enemiesToSpawn--;
            yield return new WaitForSeconds(5.0f);
        }
        _gameManager.StartCheckingForEnemies();
    }
    
    private void TotalPowerupWeightAmount()
    {
        for (int i = 0; i < _powerups.Length; i++)
        {
            _totalPowerupWeight += _powerups[i].GetComponent<Powerup>()._spawnWeight;
        }
    }

    private void PickPowerupToSpawn()
    {
        int randomNumber = UnityEngine.Random.Range(0, _totalPowerupWeight);

        foreach (GameObject powerup in _powerups)
        {
            int powerupWeight = powerup.GetComponent<Powerup>()._spawnWeight;
            if (randomNumber <= powerupWeight)
            {
                _selectedPowerup = powerup;
                break;
            }
            randomNumber -= powerupWeight;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-9, 9), 7, 0);
            PickPowerupToSpawn();
            Instantiate(_selectedPowerup, spawnArea, transform.rotation);
            float randomSpawnTime = UnityEngine.Random.Range(3, 8);
            yield return new WaitForSeconds(randomSpawnTime);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
