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
    private float _flickerTimer;

    private void Start()
    {
        _scoreText.text = $"Score: {0}";
        _gameOverText.gameObject.SetActive(false);
    }

    public void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void SetLivesSprite(int currentLives)
    {
        _livesImg.sprite = _livesSprite[currentLives];
    }

    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_flickerTimer);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerTimer);
            _gameOverText.gameObject.SetActive(true);
        }
    }
}
