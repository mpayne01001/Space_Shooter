﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    { 
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];

        if (currentLives < 1)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            _gameManager.GameOver();
            StartCoroutine(FlickerGameOverTextRoutine());
        }
    }

    IEnumerator FlickerGameOverTextRoutine()
    {
        bool displayText = false;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            _gameOverText.gameObject.SetActive(displayText);

            displayText = !displayText;
        }
    }
    public void DisplayGameOverText()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}