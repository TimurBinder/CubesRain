using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private GameObject[] _startPoints;
    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        GameObject randomStartPoint = _startPoints[Random.Range(0, _startPoints.Length)];
        cube.gameObject.transform.position = randomStartPoint.transform.position;
        cube.gameObject.SetActive(true);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_prefab);
        cube.ActivateDisable += _pool.Release;
        return cube;
    }

    private void DestroyCube(Cube cube)
    {
        cube.ActivateDisable -= _pool.Release;
        Destroy(_prefab);
    }
}
