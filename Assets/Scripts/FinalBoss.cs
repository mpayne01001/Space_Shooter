using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainLaserPrefab;
    [SerializeField]
    private GameObject _fullPowerShotPrefab;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;

    private AudioManager _audioManager;

    private UIManager _uiManager;

    private GameManager _gameManager;

    private float _fireRate;
    private float _canFire;

    private int _lives = 5;
    private int _lasersFired = 0;

    private Animator _destroyAnim;

    // Start is called before the first frame update
    void Start()
    {
        _destroyAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_destroyAnim == null)
        {
            Debug.LogError("Animation is null");
        }

        if (_audioManager == null)
        {
            Debug.LogError("Audio Manager is null");
        }

        _audioSource.clip = _laserClip;

        _canFire = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y >= 3)
        {
            this.transform.Translate(Vector3.down * 1.5f * Time.deltaTime);
        }

        if (_lasersFired <= 5)
        {
            FireLaser();
        }
        else
        {
            FireFullPowerShot();
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _lasersFired++;

            _fireRate = Random.Range(2, 5);
            _canFire = Time.time + _fireRate;
            _audioSource.Play();

            StartCoroutine(FireMainLaserRoutine());
        }
    }

    void FireFullPowerShot()
    {
        _lasersFired = 0;
        _audioSource.Play();
        Instantiate(_fullPowerShotPrefab, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
        Debug.Break();
    }

    IEnumerator FireMainLaserRoutine()
    {
        Instantiate(_mainLaserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(_mainLaserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(_mainLaserPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            DamageBoss();

            Destroy(other.gameObject);
        }
    }

    void DamageBoss()
    {
        _lives--;

        if (_lives > 0)
        {
            StartCoroutine(FlashRedRoutine());
        }
        else
        {
            _destroyAnim.SetTrigger("OnEnemyDeath");

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

            _audioManager.PlayExplosionAudio();
            _uiManager.ShowWinText();
            _gameManager.GameOver();
        }
    }

    IEnumerator FlashRedRoutine()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(.5f);

            spriteRenderer.color = Color.white;
        }
    }
}
