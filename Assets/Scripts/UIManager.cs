using System.Collections;
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
    private Text _ammoText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;

    private GameManager _gameManager;

    private bool _noAmmo;

    // Start is called before the first frame update
    void Start()
    { 
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: 15";
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

    public void UpdateAmmoCount(int ammoRemaining)
    {
        _ammoText.text = "Ammo: " + ammoRemaining;

        if (ammoRemaining == 0)
        {
            _ammoText.color = Color.red;
            _noAmmo = true;
            StartCoroutine(FlickerNoAmmoTextRoutine());
        }
        else if (ammoRemaining < 8)
        {
            _ammoText.color = Color.yellow;
        }
        else
        {
            _noAmmo = false;
            _ammoText.gameObject.SetActive(true);
            _ammoText.color = Color.white;
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

    IEnumerator FlickerNoAmmoTextRoutine()
    {
        bool displayText = false;

        while (_noAmmo)
        {
            yield return new WaitForSeconds(0.5f);

            _ammoText.gameObject.SetActive(displayText);

            displayText = !displayText;
        }
    }
    public void DisplayGameOverText()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
