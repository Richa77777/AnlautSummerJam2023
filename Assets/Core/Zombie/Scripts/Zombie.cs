using FireSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _runningSpeedOnButton;
    [SerializeField] private float _firingTime;
    [SerializeField] private float _dustLifeTime;

    private float _currentRunningSpeed;
    private bool _isFiring = false;
    private bool _isDead = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _currentRunningSpeed = _runningSpeed;
    }

    private void Update()
    {
        if (_isFiring == true)
        {
            Run();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.Rotate(0, -180, 0);
        }

        if (collision.gameObject.CompareTag("MechanicalButton"))
        {
            _currentRunningSpeed = _runningSpeedOnButton;
        }

        if (collision.GetComponent<Fire>())
        {
            Ignite();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MechanicalButton"))
        {
            _currentRunningSpeed = _runningSpeed;
        }
    }

    private void Run()
    {
        float xAxis = 0;

        if (transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            xAxis = 1;
        }

        else if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            xAxis = -1;
        }

        _rigidbody.velocity = new Vector3(xAxis * _currentRunningSpeed, _rigidbody.velocity.y);
    }

    public void Ignite()
    {
        if (_isFiring == false && _isDead == false)
        {
            _isFiring = true;
            _animator.SetBool("isFiring", true);

            StartCoroutine(FiringCycle());
        }
    }

    private void Death()
    {
        _isFiring = false;
        _isDead = true;

        _animator.SetBool("isFiring", false);
        _animator.Play("Death", 0, 0f);

        StartCoroutine(DustLifeTime(1f));
    }

    private IEnumerator FiringCycle()
    {
        yield return new WaitForSeconds(_firingTime);

        _rigidbody.velocity = new Vector3(0f, 0f);

        Death();
    }

    private IEnumerator DustLifeTime(float transparencyDuration)
    {
        yield return new WaitForSeconds(_dustLifeTime);

        Color color = _spriteRenderer.color;
        float currentAlpha = color.a;
        float targetAlpha = 0f;

        for (float t = 0; t < transparencyDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(currentAlpha, targetAlpha, t / transparencyDuration);
            _spriteRenderer.color = color;
            yield return null;
        }

        color.a = 0;
        _spriteRenderer.color = color;

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
