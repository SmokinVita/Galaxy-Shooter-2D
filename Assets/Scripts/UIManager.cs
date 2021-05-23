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

    public void Ammo(int currentAmmo)
    {
        _ammoText.text = $"Ammo: {currentAmmo}";

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
}
