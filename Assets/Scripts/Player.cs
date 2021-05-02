using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float laserCoolDown = .5f;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    private bool _canLaserFire = true;

    private int _lives = 3;

    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.Log("Spawn Manager is NULL!");
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canLaserFire)
            FireLaser();
    }

    private void FireLaser()
    {
        Vector3 offset = new Vector3(0, .8f, 0);
        Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        _canLaserFire = false;
        StartCoroutine(LaserCoolDown());
    }


    private IEnumerator LaserCoolDown()
    {
        yield return new WaitForSeconds(laserCoolDown);
        _canLaserFire = true;
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y <= -3.85f)
            transform.position = new Vector3(transform.position.x, -3.85f, 0);

        if (transform.position.x >= 11.28f)
            transform.position = new Vector3(-11.28f, transform.position.y, 0);
        else if (transform.position.x <= -11.28f)
            transform.position = new Vector3(11.28f, transform.position.y, 0);
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }
}
