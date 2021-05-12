using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isGameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene(1); //1 is the Game Scene, 0 is Main menu

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
