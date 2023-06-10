using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsController : MonoBehaviour
{
    public static PoolsController Instance;

    [SerializeField] private GameObject _smallBulletsPoolPrefab;
    [SerializeField] private GameObject _bigBulletsPoolPrefab;

    private ObjectsPool _smallBulletsPool;
    private ObjectsPool _bigBulletsPool;

    public ObjectsPool GetSmallBulletsPool => _smallBulletsPool;
    public ObjectsPool GetBigBulletsPool => _bigBulletsPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _smallBulletsPool = Instantiate(_smallBulletsPoolPrefab).GetComponent<ObjectsPool>();
        _bigBulletsPool = Instantiate(_bigBulletsPoolPrefab).GetComponent<ObjectsPool>();
    }
}
