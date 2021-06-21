using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int _enemyWeight;

    [SerializeField]
    protected float _speed = 4f;
    [SerializeField]
    protected GameObject _enemyAmmoPrefab;

    [SerializeField]
    protected GameObject _enemyShield;
    protected bool _enemyShieldActive = false;

    protected float _fireRate = 3f;
    protected float _canfire = -1;

    protected Player player;
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionAudio;

    protected bool _isDestroyed = false;

    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("Player is NULLl!");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("The Enemy Animator is Null!");

        EnableShield();
    }

    protected virtual void Update()
    {
        CalculateMovement();

        Firing();
    }

    private void EnableShield()
    {
        int randomPick = Random.Range(0, 6);
        if (randomPick <= 1)
        {
            _enemyShield.SetActive(true);
            _enemyShieldActive = true;
        }
    }

    protected virtual void Firing()
    {
        //every 3-7 seconds fire lasers
        if (Time.time > _canfire && !_isDestroyed)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_enemyAmmoPrefab, transform.position + new Vector3(0, -1.51f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    protected virtual void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.6f)
        {
            RespawnTopOfScreen();
        }
    }

    protected void RespawnTopOfScreen()
    {
        float newXPos = Random.Range(-9f, 9f);
        transform.position = new Vector3(newXPos, 7.8f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage();
                OnHit();
            }
        }

        /*if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            //onHit is called from laser script
        }*/

        if (collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
            OnHit();
        }
    }

    public virtual void OnHit()
    {
        if (!_enemyShieldActive)
        {
            if (player != null)
                player.AddScore();

            _isDestroyed = true;
            Destroy(GetComponent<Collider2D>());

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            AudioSource.PlayClipAtPoint(_explosionAudio, transform.position + new Vector3(0, 0, -10), 1f);

            Destroy(this.gameObject, 2.37f);
        }
        else if(_enemyShieldActive)
        {
            _enemyShieldActive = false;
            _enemyShield.SetActive(false);
        }
    }
}
