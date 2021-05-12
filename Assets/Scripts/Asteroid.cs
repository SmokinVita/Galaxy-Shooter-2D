using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _rotationSpeed = 3f;
    [SerializeField]
    private GameObject _explositionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Spawn Manager is NULL!");
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Laser"))
        {
            
            _spawnManager.StartSpawning();
            Instantiate(_explositionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(this.gameObject, .15f);
        }

    }
}
