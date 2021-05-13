using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player's Info")]
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _explosionPrefab;

    [Header("Laser Info")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float laserCoolDown = .5f;
    [SerializeField]
    private int _ammo;
    private bool _canLaserFire = true;


    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [Header("Power Ups")]
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private float _powerDown = 5f;
    [SerializeField]
    private float _speedMultipler = 2f;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private int _currentShieldStrength;
    [SerializeField]
    private int _maxShieldStrength = 3;

    private Renderer _shieldRenderer;

    private bool _isSpeedBoostActive = false;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserShotAudio;
    [SerializeField]
    private AudioClip _explosionAudio;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.Log("Spawn Manager is NULL!");

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
            Debug.LogError("UI manager is NULL!");

        _shieldRenderer = _shield.GetComponent<Renderer>();
        if (_shieldRenderer == null)
            Debug.LogError("The Renderer is NULL!");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("Audio Source is NULL!");

        _uiManager.Ammo(_ammo);
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canLaserFire && _ammo > 0)
            FireLaser();
        else if (_ammo == 0)
        {
            _uiManager.Ammo(_ammo);
            Debug.Log("Out of Ammo!");
        }



    }

    private void FireLaser()
    {
        _canLaserFire = false;
       //if ammo is greater than 0 canLaserFire is true
       //fire take away ammo
       //display on screen
       //else if 
       //flash warning need to reload

        Vector3 offset = new Vector3(0, 1.14f, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShot, transform.position + offset, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }

        _ammo--;
        _uiManager.Ammo(_ammo);

        StartCoroutine(LaserCoolDown());

        _audioSource.clip = _laserShotAudio;
        _audioSource.Play();
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

        //while left shift is down increase speed while fuel
        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(direction * (_speed * 3) * Time.deltaTime);
        else
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
        if (_isShieldActive)
        {
            if (_currentShieldStrength > 0)
            {
                _currentShieldStrength--;
                ShieldDisplayStrength(_currentShieldStrength);
                return;
            }
            else
            {
                _isShieldActive = false;
                _shield.SetActive(false);
            }
        }

        _lives--;

        _uiManager.SetLivesSprite(_lives);

        if (_lives == 2)
            _rightEngine.SetActive(true);
        else if (_lives == 1)
            _leftEngine.SetActive(true);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOver();
            _audioSource.clip = _explosionAudio;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
            Destroy(gameObject);
        }
    }

    public void TripleShotActivate()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerDown);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActivate()
    {
        _speed *= _speedMultipler;
        StartCoroutine(SpeedBoostDeactivate());
    }

    IEnumerator SpeedBoostDeactivate()
    {
        yield return new WaitForSeconds(_powerDown);
        _speed /= _speedMultipler;
    }

    public void ShieldActivate()
    {
        if (_currentShieldStrength == _maxShieldStrength)
        {
            return;
        }
        else
        {
            _isShieldActive = true;
            _shield.SetActive(true);
            _currentShieldStrength++;
            ShieldDisplayStrength(_currentShieldStrength);
        }
    }

    private void ShieldDisplayStrength(int strength)
    {

        switch (strength)
        {
            case 1:
                _shieldRenderer.material.color = new Color(1, 1, 1, .25f);
                break;
            case 2:
                _shieldRenderer.material.color = new Color(1, 1, 1, .50f);
                break;
            case 3:
                _shieldRenderer.material.color = new Color(1, 1, 1, 1);
                break;
        }
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.SetScoreText(_score);
    }
}
