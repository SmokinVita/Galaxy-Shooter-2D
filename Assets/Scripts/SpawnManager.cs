using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;
    private GameObject _selectedPowerup;


    private bool _stopSpawning = false; 

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 spawnArea = new Vector3(UnityEngine.Random.Range(-9, 9), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnArea, transform.rotation);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    
    private void PickPowerupToSpawn()
    {

        int totalWeight = 0;
        for (int i = 0; i < _powerups.Length; i++)
        {
            totalWeight += _powerups[i].GetComponent<Powerup>()._spawnWeight;
        }

        int randomNumber = UnityEngine.Random.Range(0, totalWeight);

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
