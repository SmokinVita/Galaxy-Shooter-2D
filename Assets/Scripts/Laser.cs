using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private bool _isEnemy = false;

    void Update()
    {
        if (_isEnemy == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemy = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isEnemy)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    Destroy(gameObject);
                }
            }
            else if (collision.CompareTag("Powerup"))
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if(enemy !=null)
                {
                    enemy.OnHit();
                    Destroy(gameObject);
                }
            }
        }
    }
}
