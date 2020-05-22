using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;
    [SerializeField]
    GameObject _enemyContainer;
    [SerializeField]
    GameObject[] _powerups;
    [SerializeField]
    GameObject _asteroidPrefab;
    [SerializeField]
    private float _enemySpawnTime = 5;

    Vector3[] _enemySpawnPattern;

    private bool _stopSpawningAll;
    private bool _stopSpawningEnemies;

    [SerializeField]
    private int _wave = 1;
    [SerializeField]
    private int _enemiesPerWave = 3;
    [SerializeField]
    private int _enemiesSpawned = 0;
    [SerializeField]
    private int _enemiesDestroyed = 0;

    UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _enemySpawnPattern = new[] {
        new Vector3(Random.Range(-9.5f, 9.5f), 7, 0),
        new Vector3(-11.5f, 5.5f, 0),
        new Vector3(11.5f, 5.5f, 0)
    };

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void StartSpawning()
    {
        _stopSpawningAll = false;
        _stopSpawningEnemies = false;

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSprayShotRoutine());
    }

    private void SpawnAsteroid()
    {
        Instantiate(_asteroidPrefab, _asteroidPrefab.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemiesSpawned == _wave * _enemiesPerWave)
        {
            _stopSpawningEnemies = true;
        }
    }

    //spawn game objects every 5 seconds
    //Create a coroutine of type Ienumerator -- Valid Events
    //while loop
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawningAll && !_stopSpawningEnemies)
        {
            var spawnPattern = Random.Range(0, 3);
            GameObject newEnemy = Instantiate(_enemyPrefab, _enemySpawnPattern[spawnPattern], Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            var enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                switch (spawnPattern)
                {
                    case 0:
                        enemy.EnemyType = "Normal";
                        break;
                    case 1:
                        enemy.EnemyType = "Left";
                        break;
                    case 2:
                        enemy.EnemyType = "Right";
                        break;
                }
            }
            _enemiesSpawned++;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawningAll)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            int randomPowerup = Random.Range(0, 5);
            Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
        }
    }

    //10% chance to spawn every 3 seconds
    IEnumerator SpawnSprayShotRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawningAll)
        {
            yield return new WaitForSeconds(3);
            //Get random value 0-9
            int randomValue = Random.Range(0, 10);
            //if value is 0, spawn spray shot
            if (randomValue == 0)
                Instantiate(_powerups[5], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawningAll = true;
    }

    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;

        if (_enemiesDestroyed == _wave * _enemiesPerWave)
        {
            _stopSpawningAll = true;
            BeginNextWave();
        }
    }

    private void BeginNextWave()
    {
        _wave++;
        _enemiesDestroyed = 0;
        _enemiesSpawned = 0;

        StartCoroutine(NextWaveRoutine());
    }

    IEnumerator NextWaveRoutine()
    {
        _uiManager.ShowWaveText(_wave);

        yield return new WaitForSeconds(2);

        SpawnAsteroid();
    }
}
