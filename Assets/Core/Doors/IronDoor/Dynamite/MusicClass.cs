using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }
}