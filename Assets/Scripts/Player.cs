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
    private float _speedBoost = 3f;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _explosionPrefab;

    [Header("Thruster Info")]
    [SerializeField]
    private float _maxTemp = 5f;
    [SerializeField]
    private float _currentEngineTemp;
    [SerializeField]
    private bool _isEngineOverHeated = false;
    

    [Header("Laser Info")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float laserCoolDown = .5f;
    [SerializeField]
    private int _ammo;
    private int _maxAmmo = 15;
    private bool _canLaserFire = true;


    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private CameraShake _shakeCamera;

    [Header("Power Ups")]
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _missileShot;
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

    [Header("Magnet Info")]
    [SerializeField]
    private float _powerupSpeed;
    [SerializeField]
    private float _maxMagnetPower = 3f;
    private float _currentMagnetPower;
    private bool _canUseMagnet = true;

    private Renderer _shieldRenderer;

    private bool _isTripleShotActive = false;
    private bool _isMissileShotActive = false;
    private bool _isShieldActive = false;

    private Vector3 _scaleUP = new Vector3(1,1,1);
    private Vector3 _originalScale;

    [Header("Audio")]
    [SerializeField]
    private AudioClip _laserShotAudio;
    [SerializeField]
    private AudioClip _explosionAudio;
    private AudioSource _audioSource;
    

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _originalScale = transform.localScale;
        _currentMagnetPower = _maxMagnetPower;

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

        _shakeCamera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_shakeCamera == null)
            Debug.LogError("Camera Shake is NULL!");


        _uiManager.Ammo(_ammo, _maxAmmo);
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canLaserFire)
            FireLaser();

        if (Input.GetKey(KeyCode.C) && _canUseMagnet)
        {
            PullPowerups();
        }

        _uiManager.MagnetPowerGauge(_currentMagnetPower / _maxMagnetPower);
    }

    private void PullPowerups()
    {
        _currentMagnetPower -= Time.deltaTime;

        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");

        foreach (var powerup in powerups)
        {
            powerup.transform.position = Vector2.MoveTowards
                (powerup.transform.position, transform.position, _powerupSpeed * Time.deltaTime);
        }

        if(_currentMagnetPower < 0)
        {
            _canUseMagnet = false;
            StartCoroutine(ReenergizeMagnetRoutine());
        }
    }

    private IEnumerator ReenergizeMagnetRoutine()
    {
        while (_canUseMagnet == false)
        {
            yield return new WaitForSeconds(.5f);
            _currentMagnetPower += Time.deltaTime;
            if (_currentMagnetPower >= _maxMagnetPower)
            {
                _canUseMagnet = true;
            }
        }
    }

    private void FireLaser()
    {
        if (_ammo > 0)
        {
            _canLaserFire = false;

            Vector3 offset = new Vector3(0, 1.14f, 0);

            if (_isTripleShotActive)
            {
                Instantiate(_tripleShot, transform.position + offset, Quaternion.identity);
                _ammo--;
            }
            else if (_isMissileShotActive)
            {
                Instantiate(_missileShot, transform.position + offset, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
                _ammo--;
            }


            _uiManager.Ammo(_ammo, _maxAmmo);

            StartCoroutine(LaserCoolDown());

            _audioSource.clip = _laserShotAudio;
            _audioSource.Play();
        }
        else
        {
            _uiManager.Ammo(_ammo, _maxAmmo);
        }
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

        if (Input.GetKey(KeyCode.LeftShift) && !_isEngineOverHeated)
        {
            transform.Translate(direction * (_speed * _speedBoost) * Time.deltaTime);
            _currentEngineTemp += Time.deltaTime;

            if (_currentEngineTemp >= _maxTemp)
            {
                _isEngineOverHeated = true;
                StartCoroutine(EngineCoolDownRoutine());
            }
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
            if (_currentEngineTemp > 0 && !_isEngineOverHeated)
                _currentEngineTemp -= Time.deltaTime;
        }

        
        _uiManager.ThrustTempGauge(_currentEngineTemp/_maxTemp, _isEngineOverHeated);


        if (transform.position.y >= 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y <= -3.85f)
            transform.position = new Vector3(transform.position.x, -3.85f, 0);

        if (transform.position.x >= 11.28f)
            transform.position = new Vector3(-11.28f, transform.position.y, 0);
        else if (transform.position.x <= -11.28f)
            transform.position = new Vector3(11.28f, transform.position.y, 0);
    }

    private IEnumerator EngineCoolDownRoutine()
    {
        while (_isEngineOverHeated)
        {
            yield return new WaitForSeconds(1f);
            _currentEngineTemp -=.5f;
            if (_currentEngineTemp <= 0)
                _isEngineOverHeated = false;
        }
    }

    public void Heal()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.SetLivesSprite(_lives);

            if (_lives == 3)
                _rightEngine.SetActive(false);
            else if (_lives == 2)
                _leftEngine.SetActive(false);
        }
        else
            return;
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            if (_currentShieldStrength > 0)
            {
                _currentShieldStrength--;
                ShieldDisplayStrength();
                return; //jumps out of method to now damage player
            }
            else
            {
                _isShieldActive = false;
                _shield.SetActive(false);
            }
        }

        _shakeCamera.ShakeCamera();
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

    public void RefillAmmo()
    {
        if (_ammo != _maxAmmo)
        {
            _ammo = _maxAmmo;
            _uiManager.Ammo(_ammo, _maxAmmo);
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

    public void MissileActive()
    {
        _isMissileShotActive = true;
        StartCoroutine(MissileShotPowerDownRoutine());
    }

    private IEnumerator MissileShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isMissileShotActive = false;
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
            ShieldDisplayStrength();
        }
    }

    private void ShieldDisplayStrength()
    {
        switch (_currentShieldStrength)
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
            default:
                _shieldRenderer.gameObject.SetActive(false);
                break;
        }
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.SetScoreText(_score);
    }

    public void NegitivePowerup()
    {
        transform.localScale = _scaleUP;
        StartCoroutine(ScaleDownRoutine());
    }

    private IEnumerator ScaleDownRoutine()
    {
        yield return new WaitForSeconds(_powerDown);
        transform.localScale = _originalScale;
    }
}
