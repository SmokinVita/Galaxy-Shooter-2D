using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _enemyID;
    //0 = basic enemy
    //1 = basic enemy with new movement type
    //2 = enemy is zigzag movement and new wep.


    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _scatterBombPrefab;

    [SerializeField]
    private Transform[] _wayPointsToFollow;
    [SerializeField]
    private GameObject _wayPointPrefab;
    private int _currentWayPoint = 0;
    private GameObject _wayPoint;

    private float _fireRate = 3f;
    private float _canfire = -1;

    private Player player;
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionAudio;

    private bool _isDestroyed = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("Player is NULLl!");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("The Enemy Animator is Null!");

        if (_enemyID == 2)
        {
            _wayPoint = Instantiate(_wayPointPrefab, transform.position, Quaternion.identity);

            _wayPointsToFollow = new Transform[_wayPoint.transform.childCount];
            for (int i = 0; i < _wayPoint.transform.childCount; i++)
            {
                _wayPointsToFollow[i] = _wayPoint.transform.GetChild(i);
            }
        }

    }

    void Update()
    {
        CalculateMovement();

        Firing();
    }

    private void Firing()
    {
        //every 3-7 seconds fire lasers
        if (Time.time > _canfire && !_isDestroyed)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            if (_enemyID == 0)
            {

                GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -1.386f, 0), Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
            else if (_enemyID == 2)
            {
                for (int fireAngle = 0; fireAngle < 360; fireAngle += 30)
                {
                    var newBullet = Instantiate(_scatterBombPrefab, transform.position, Quaternion.identity);
                    newBullet.transform.eulerAngles = Vector3.forward * fireAngle;
                }
            }
        }
    }

    private void CalculateMovement()
    {
        if (_enemyID == 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (_enemyID == 2)
        {
            var direction = _wayPointsToFollow[_currentWayPoint].position - transform.position;
            transform.localRotation = Quaternion.LookRotation(Vector3.forward, -direction);
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y <= -5.6f)
        {
            RespawnTopOfScreen();
        }
    }

    private void RespawnTopOfScreen()
    {
        float newXPos = Random.Range(-9f, 9f);
        transform.position = new Vector3(newXPos, 7.8f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Waypoint"))
        {
            _currentWayPoint += 1;
            if (_currentWayPoint >= _wayPointsToFollow.Length)
            {
                _currentWayPoint = 0;
                RespawnTopOfScreen();
                collision.gameObject.transform.parent.position = gameObject.transform.position;
            }
        }

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

        }

        if (collision.CompareTag("Missile"))
        {

            Destroy(collision.gameObject);
            OnDeath();

        }
    }

    public void OnDeath()
    {

        if (player != null)
            player.AddScore();

        _isDestroyed = true;
        Destroy(GetComponent<Collider2D>());

        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;

        AudioSource.PlayClipAtPoint(_explosionAudio, transform.position + new Vector3(0, 0, -10), 1f);

        Destroy(this.gameObject, 2.37f);
        Destroy(_wayPoint, 2.37f);
    }
}
