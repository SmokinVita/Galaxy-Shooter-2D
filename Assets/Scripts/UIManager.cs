using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _gameResetText;
    [SerializeField]
    private Text _outOfAmmoText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private float _flickerTimer;
    [SerializeField]
    private Image _heatGauge;
    [SerializeField]
    private Text _currentWave;
    [SerializeField]
    private Slider _magnetPowerGauge;

    [SerializeField]
    private GameManager _gameManager;

    private void Start()
    {
        _scoreText.text = $"Score: {0}";
        _gameOverText.gameObject.SetActive(false);
        _gameResetText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.Log("Game Manager is NULL!");
    }

    public void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void SetLivesSprite(int currentLives)
    {
        if (currentLives < 0)
            return;
        _livesImg.sprite = _livesSprite[currentLives];

    }

    public void Ammo(int currentAmmo, int maxAmmo)
    {
        _ammoText.text = $"Ammo: {currentAmmo} / {maxAmmo}";

        if (currentAmmo <= 0)
            _outOfAmmoText.gameObject.SetActive(true);
        else if (currentAmmo > 0)
            _outOfAmmoText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _gameResetText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_flickerTimer);
            _gameOverText.gameObject.SetActive(false);
            _gameResetText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerTimer);
            _gameOverText.gameObject.SetActive(true);
            _gameResetText.gameObject.SetActive(true);
        }
    }

    public void MagnetPowerGauge(float currentMagnetPower)
    {
        _magnetPowerGauge.value = currentMagnetPower;
    }

    public void ThrustTempGauge(float currentTemp, bool _isEngineOverHeated)
    {

        _heatGauge.fillAmount = currentTemp;


        if (_isEngineOverHeated)
        {
            _heatGauge.color = Color.red;
        }
        else
        {
            _heatGauge.color = Color.Lerp(Color.green, Color.red, currentTemp);
        }
    }

    public void StartOfWave(int currentWave)
    {
        _currentWave.gameObject.SetActive(true);
        _currentWave.text = $"Wave {currentWave} Incoming!";

        StartCoroutine(WaveRoutine());
    }

    public void BossIncoming()
    {
        _currentWave.gameObject.SetActive(true);
        _currentWave.text = $"Boss Incoming!";

        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(2f);
        _currentWave.gameObject.SetActive(false);
    }
}
