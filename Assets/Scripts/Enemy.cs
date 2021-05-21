using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;

    private float _fireRate = 3f;
    private float _canfire = -1;

    private Player player;
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionAudio;

    public bool _isDestroyed = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("Player is NULLl!");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("The Enemy Animator is Null!");
    }

    void Update()
    {
        CalculateMovement();

        //every 3-7 seconds fire lasers
        if(Time.time > _canfire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.386f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.6f)
        {
            float newXPos = Random.Range(-9f, 9f);
            transform.position = new Vector3(newXPos, 7.8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage();
                OnDeath();
            }
        }

        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);

            if (player != null)
                player.AddScore();

            OnDeath();

            
        }

        if (collision.CompareTag("Missile"))
        {

            Destroy(collision.gameObject);
            OnDeath();
            
        }
    }

    public void OnDeath()
    {
        _isDestroyed = true;
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;

        AudioSource.PlayClipAtPoint(_explosionAudio, transform.position + new Vector3(0, 0, -10), 1f);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.37f);
    }
}
