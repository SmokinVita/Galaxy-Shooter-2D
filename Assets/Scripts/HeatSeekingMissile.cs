using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingMissile : MonoBehaviour
{

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _isEnemyDetected = false;
    private bool _isEnemyDestroyed = false;

    private void FindEnemy()
    {
        Collider2D _targets = Physics2D.OverlapCircle(transform.position, 2.5f);
        if (_targets.CompareTag("Enemy"))
        {
            Enemy enemyCheck = _targets.GetComponent<Enemy>();
            if (enemyCheck != null)
            {
                _isEnemyDestroyed = enemyCheck._isDestroyed;

                if (_target == null || _isEnemyDestroyed)
                {
                    _isEnemyDetected = false;
                }
                else if (!_isEnemyDetected && !_isEnemyDestroyed)
                {
                    _target = _targets.gameObject;
                    _isEnemyDetected = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        FindEnemy();

        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (!_isEnemyDetected)
        {
            transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
        }
        else if (_isEnemyDetected)
        {

            Vector3 newDirection = _target.transform.position - transform.position;
            transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.LookRotation(transform.forward, newDirection);
        }

        if (transform.position.y > 7.8f || transform.position.y < -5.6f || transform.position.x < -11.28f || transform.position.x > 11.28f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Instantiate(_explosionPrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

}
