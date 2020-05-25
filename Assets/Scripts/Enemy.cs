using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    private Animator _destroyAnim;

    private AudioManager _audioManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;

    private float _fireRate;
    private float _canFire;
    private bool _smartLaserFired;
    private bool _powerUpLaserFired;

    [SerializeField]
    GameObject _shield;
    private bool _enemyHasShield;

    public string EnemyType;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyAnim = GetComponent<Animator>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _audioSource = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        if (_destroyAnim == null)
        {
            Debug.LogError("Animation is null");
        }

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        _audioSource.clip = _laserClip;

        _canFire = -1;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        FireLaser();

        CheckForPowerupsAndFireLaser();

        if (this.transform.position.y <= _player.transform.position.y - 4 && !_smartLaserFired)
        {
            _smartLaserFired = true;
            FireSmartLaser();
        }

        if (this.transform.position.y > _player.transform.position.y)
        {
            _smartLaserFired = false;
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3, 8);
            _canFire = Time.time + _fireRate;
            _audioSource.Play();
            //Get lasers and set them to be enemy lasers
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            lasers[0].AssignEnemyLaser();
            lasers[1].AssignEnemyLaser();
        }
    }

    void FireSmartLaser()
    {
        _audioSource.Play();
        //Get lasers and set them to be enemy lasers
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        lasers[0].AssignEnemySmartLaser();
        lasers[1].AssignEnemySmartLaser();
    }

    void FirePowerupLaser()
    {
        _audioSource.Play();
        //Get lasers and set them to be enemy lasers
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        lasers[0].AssignEnemyLaser();
        lasers[1].AssignEnemyLaser();
    }

    void CalculateMovement()
    {
        switch (EnemyType)
        {
            case "Normal":
                MoveDown();
                break;
            case "Left":
                MoveRight();
                break;
            case "Right":
                MoveLeft();
                break;
        }

    }

    //Fires at 1st powerup it sees
    void CheckForPowerupsAndFireLaser()
    {
        var powerups = _spawnManager.GetActivePowerups();

        if (powerups != null)
        {
            Debug.Log(powerups.Length);
            foreach (var powerup in powerups)
            {
                Debug.Log(powerup.position.x);
                if (powerup.position.x >= this.transform.position.x - 0.5f
                    && powerup.position.x <= this.transform.position.x + 0.5f
                    && powerup.position.y <= this.transform.position.y - 1.5f
                    && !_powerUpLaserFired)
                {
                    _powerUpLaserFired = true;
                    FirePowerupLaser();
                }
            }
        }
    }

    void MoveDown()
    {
        //move down at 4m/s
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 7, 0);
        }
    }

    void MoveLeft()
    {
        //move down at 4m/s
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.x <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y - 2.5f, 0);
        }

        if (transform.position.y <= -6f)
        {
            transform.position = new Vector3(11.5f, 5.5f, 0);
        }
    }

    void MoveRight()
    {
        //move down at 4m/s
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y - 2.5f, 0);
        }

        if (transform.position.y <= -6f)
        {
            transform.position = new Vector3(-11.5f, 5.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit " + other.transform.name);
        //if other is Player, damage player then destroy us
        if (other.tag == "Player")
        {
            var player = other.transform.GetComponent<Player>();
            if (player != null)
                player.Damage();

            if (_enemyHasShield)
            {
                _shield.SetActive(false);
                _enemyHasShield = false;
            }
            else
            {
                _destroyAnim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(this.gameObject, 2.8f);
                _audioManager.PlayExplosionAudio();

                _spawnManager.EnemyDestroyed();
            }
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_enemyHasShield)
            {
                _shield.SetActive(false);
                _enemyHasShield = false;
            }
            else
            {
                _destroyAnim.SetTrigger("OnEnemyDeath");
                _speed = 0;

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);

                _audioManager.PlayExplosionAudio();

                if (_player != null)
                    _player.AddScore(10);

                _spawnManager.EnemyDestroyed();
            }
        }
    }

    public void AddEnemyShield()
    {
        _enemyHasShield = true;
        _shield.SetActive(true);
    }
}
