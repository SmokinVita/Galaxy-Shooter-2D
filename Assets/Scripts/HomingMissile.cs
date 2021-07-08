using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _explosion;
    private GameObject[] _targets;
    private  GameObject _selectedTarget;
    

    void Start()
    {
        FindEnemy();
    }

    private void FindEnemy()
    {
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;

        if (_targets != null)
        {
            foreach (var target in _targets)
            {
                Vector2 diff = target.transform.position - transform.position;
                float currentDistance = diff.sqrMagnitude;
                if (currentDistance < distance)
                {
                    _selectedTarget = target;
                    distance = currentDistance;
                }
            }
        }
    }

    void Update()
    {

        if (_selectedTarget == null)
        {
            FindEnemy();
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }
        else
        {
            Vector3 direction = _selectedTarget.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, direction);
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.OnHit();
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
