using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            var explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
            Destroy(explosion.gameObject, 2.7f);
            _audioManager.PlayExplosionAudio();
        }
    }
}
