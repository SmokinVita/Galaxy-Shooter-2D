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



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3f);
    }

    void Update()
    {
        FindEnemy();
        CalculateMovement();
    }

    private void CalculateMovement()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (_isEnemyDetected)
        {
            Vector3 newDirection = _target.transform.position - transform.position;

            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            transform.localRotation = Quaternion.LookRotation(transform.forward, newDirection);
        }


        if (transform.position.y > 7.8f || transform.position.y < -5.6f || transform.position.x < -11.28f || transform.position.x > 11.28f)
            Destroy(gameObject);
    }

    private void FindEnemy()
    {

        Collider2D _targets = Physics2D.OverlapCircle(transform.position, 3f);
        if (_targets.CompareTag("Enemy"))
        {
            _target = _targets.gameObject;
            _isEnemyDetected = true;
        }
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
