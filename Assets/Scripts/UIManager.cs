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
    private Text _thrusterText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;

    private GameManager _gameManager;

    private bool _noAmmo;
    private bool _noThrusters;

    public int MaxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        _waveText.text = "Wave 1";
        _scoreText.text = "Score: " + 0;
        _thrusterText.text = "Thrusters: 100";
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }

        ShowWaveText(1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowWaveText(int wave)
    {
        StartCoroutine(WaveTextRoutine(wave));
    }

    IEnumerator WaveTextRoutine(int wave)
    {
        _waveText.text = "Wave " + wave;
        _waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        _waveText.gameObject.SetActive(false);

    }

    public void InitializeMaxAmmo(int maxAmmo)
    {
        MaxAmmo = maxAmmo;
        _ammoText.text = "Ammo: " + MaxAmmo + " / " + MaxAmmo;
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
        _ammoText.text = "Ammo: " + ammoRemaining + " / " + MaxAmmo;

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

    public void UpdateThrusterText(int remainingThrusters)
    {
        _thrusterText.text = "Thrusters: " + remainingThrusters;

        if (remainingThrusters == 0)
        {
            _thrusterText.color = Color.red;
            _noThrusters = true;
            StartCoroutine(FlickerNoThrustersTextRoutine());
        }
        else if (remainingThrusters <= 50)
        {
            _noThrusters = false;
            _thrusterText.gameObject.SetActive(true);
            _thrusterText.color = Color.yellow;
        }
        else
        {
            _noThrusters = false;
            _thrusterText.gameObject.SetActive(true);
            _thrusterText.color = Color.white;
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

    IEnumerator FlickerNoThrustersTextRoutine()
    {
        bool displayText = false;

        while (_noThrusters)
        {
            yield return new WaitForSeconds(0.5f);

            _thrusterText.gameObject.SetActive(displayText);

            displayText = !displayText;
        }
    }

    public void DisplayGameOverText()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
