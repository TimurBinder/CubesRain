using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Transform[] _startPoints;
    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetCube(_repeatRate));
    }

    private IEnumerator GetCube(float delay)
    {
        while (enabled)
        {
            Cube cube = _pool.Get();
            cube.DisableActivated += _pool.Release;
            yield return new WaitForSeconds(delay);
        }
    }

    private void ActionOnGet(Cube cube)
    {
        Transform randomStartPoint = _startPoints[Random.Range(0, _startPoints.Length)];
        cube.gameObject.transform.position = randomStartPoint.position;
        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.DisableActivated -= _pool.Release;
        cube.gameObject.SetActive(false);
    }
}
