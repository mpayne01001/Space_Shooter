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

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;

    private float _fireRate;
    private float _canFire;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyAnim = GetComponent<Animator>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        if (_destroyAnim == null)
        {
            Debug.LogError("Animation is null");
        }

        _audioSource.clip = _laserClip;

        _canFire = -1;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

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

    void CalculateMovement()
    {
        //move down at 4m/s
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f)
        {
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 7, 0);
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

            _destroyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
            _audioManager.PlayExplosionAudio();
        }
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _destroyAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

            _audioManager.PlayExplosionAudio();

            if (_player != null)
                _player.AddScore(10);
        }
    }
}
