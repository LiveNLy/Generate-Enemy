using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private SpawnPoint[] _spawnPoints;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolDefaultCapacity = 5;
    [SerializeField] private int _poolMaxSize = 12;

    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            actionOnGet: (enemy) => SetEnemyTransformParameters(enemy),
            createFunc: () => Instantiate(_enemyPrefab),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy(_repeatRate));
    }

    public void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
        enemy.Releasing -= ReleaseEnemy;
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void SetEnemyTransformParameters(Enemy enemy)
    {
        enemy.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
        enemy.gameObject.SetActive(true);
        enemy.StartMoving(SetRandomDirection().normalized);
        enemy.Releasing += ReleaseEnemy;
    }

    private Vector3 SetRandomDirection()
    {
        float randomDirectionRange = 1f;
        Vector3 directon = new Vector3();

        directon.x = Random.Range(-randomDirectionRange, randomDirectionRange);
        directon.z = Random.Range(-randomDirectionRange, randomDirectionRange);

        return directon;
    }

    private IEnumerator SpawnEnemy(float seconds)
    {
        var wait = new WaitForSeconds(seconds);

        while (enabled)
        {
            GetEnemy();
            yield return wait;
        }
    }
}
