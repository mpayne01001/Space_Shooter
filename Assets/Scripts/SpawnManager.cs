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
    private float _enemySpawnTime = 5;

    private bool _stopSpawning;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    //spawn game objects every 5 seconds
    //Create a coroutine of type Ienumerator -- Valid Events
    //while loop
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            int randomPowerup = Random.Range(0, 5);
            Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-9.5f, 9.5f), 7, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
