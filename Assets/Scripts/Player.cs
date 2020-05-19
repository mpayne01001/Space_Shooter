using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public or private reference
    //data type (int, float, bool, string)
    //every variable has a name
    //optional value assigned
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _tempSpeed = 0;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;

    [SerializeField]
    GameObject _shield;

    [SerializeField]
    private int _score;

    UIManager _uiManager;
    AudioManager _audioManager;

    [SerializeField]
    private GameObject[] _engineFires;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //take the current position and assign to start (0 ,0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is null");
        }

        if (_audioSource != null)
        {
            _audioSource.clip = _laserClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Enemy_Laser")
        {
            Destroy(other.transform.parent.gameObject);
            Damage();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (_isSpeedBoostActive)
        {
            _tempSpeed = _speed * 2;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _tempSpeed = _speed + 2;
        }
        else
        {
            _tempSpeed = _speed;
        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _tempSpeed * Time.deltaTime);

        //Clamping
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
        //play laser audio clip
    }

    public void Damage()
    { 
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _engineFires[Random.Range(0, 2)].SetActive(true);
        } 
        else if (_lives == 1)
        {
            _engineFires[0].SetActive(true);
            _engineFires[1].SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            //Communicate with spawn manager
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _audioManager.PlayExplosionAudio();
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);

        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;

        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);

        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;

        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;

        _uiManager.UpdateScoreText(_score);
    }
    //method to add 10 to score
    //communicate with UI to add score
}
