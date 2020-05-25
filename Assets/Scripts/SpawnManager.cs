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
    GameObject _powerupContainer;
    [SerializeField]
    GameObject[] _powerups;
    [SerializeField]
    GameObject _asteroidPrefab;
    [SerializeField]
    GameObject _finalBossPrefab;
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
        StartCoroutine(SpawnHomingShotRoutine());
    }

    private void SpawnAsteroid()
    {
        Instantiate(_asteroidPrefab, _asteroidPrefab.transform.position, Quaternion.identity);
    }

    private void SpawnFinalBoss()
    {
        Instantiate(_finalBossPrefab, new Vector3(0, 7f, 0), Quaternion.identity);
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
                //25% chance for enemy to have shields
                int randomValue = Random.Range(1, 5);
                if (randomValue == 1)
                    enemy.AddEnemyShield();
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
            int randomPowerup = Random.Range(0, 10);
            switch (randomPowerup)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    randomPowerup = 3; //Ammo
                    break;
                case 4:
                    randomPowerup = 0; //Triple Shot
                    break;
                case 5:
                case 6:
                    randomPowerup = 1; //Speed
                    break;
                case 7:
                    randomPowerup = 4; //Health
                    break;
                case 8:
                    randomPowerup = 2; //Shield
                    break;
                default:
                    randomPowerup = 5; //Slow
                    break;
            }
            var newPowerup = Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
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
            {
                var sprayShot = Instantiate(_powerups[6], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
                sprayShot.transform.parent = _powerupContainer.transform;
            }
        }
    }

    //5% chance to spawn every 3 seconds
    IEnumerator SpawnHomingShotRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawningAll)
        {
            yield return new WaitForSeconds(3);
            //Get random value 0-19
            int randomValue = Random.Range(0, 20);
            //if value is 0, spawn homing shot
            if (randomValue == 0)
            {
                var homingShot = Instantiate(_powerups[7], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
                homingShot.transform.parent = _powerupContainer.transform;
            }
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

        if (_wave > 3)
        {
            StartCoroutine(BossWaveRoutine());
        }
        else
        {
            StartCoroutine(NextWaveRoutine());
        }
    }

    IEnumerator NextWaveRoutine()
    {
        _uiManager.ShowWaveText(_wave);

        yield return new WaitForSeconds(2);

        SpawnAsteroid();
    }

    IEnumerator BossWaveRoutine()
    {
        _uiManager.ShowWaveText(_wave);

        yield return new WaitForSeconds(2);

        SpawnFinalBoss();
    }

    public Transform GetClosestEnemy(Vector3 laserPosition)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Enemy potentialTarget in _enemyContainer.GetComponentsInChildren<Enemy>())
        {
            if (potentialTarget.GetComponent<Collider2D>() == null)
                continue;
            Transform transform = potentialTarget.transform;
            Vector3 directionToTarget = transform.position - laserPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }

    public Transform[] GetActivePowerups()
    {
        return _powerupContainer.GetComponentsInChildren<Transform>();
    }
}
