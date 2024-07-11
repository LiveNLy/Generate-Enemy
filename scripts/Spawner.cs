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
            actionOnGet: (enemy) => SetEnemy(enemy),
            createFunc: () => Instantiate(_enemyPrefab),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        FindSpawners();
        StartCoroutine(SpawnEnemy(_repeatRate));
    }

    public void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void SetEnemy(Enemy enemy)
    {
        enemy.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
        enemy.transform.rotation *= Quaternion.Euler(0, Random.Range(10, 360), 0);
        enemy.gameObject.SetActive(true);
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

    [ContextMenu(nameof(FindSpawners))]
    private void FindSpawners()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }
}
