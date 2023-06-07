using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsController : MonoBehaviour
{
    public static PoolsController Instance;

    [SerializeField] private GameObject _particlesPoolPrefab;
    [SerializeField] private GameObject _puddlesPoolPrefab;

    private ObjectsPool _particlesPool;
    private ObjectsPool _puddlesPool;

    public ObjectsPool GetParticlesPool => _particlesPool;
    public ObjectsPool GetPuddlesPool => _puddlesPool;

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

        _particlesPool = Instantiate(_particlesPoolPrefab).GetComponent<ObjectsPool>();
        _puddlesPool = Instantiate(_puddlesPoolPrefab).GetComponent<ObjectsPool>();
    }
}
