using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _enemyPrefab;
    private GameObject _selectedEnemy;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;
    private GameObject _selectedPowerup;

    private int _totalPowerupWeight = 0;
    private int _totalEnemyWeight = 0;

    private bool _stopSpawning = false; 

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        TotalEnemyWeightAmount();

        StartCoroutine(SpawnPowerupRoutine());
        TotalPowerupWeightAmount();
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

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-9, 9), 7, 0);
            PickEnemyToSpawn();
            GameObject newEnemy = Instantiate(_selectedEnemy, spawnArea, transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
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
