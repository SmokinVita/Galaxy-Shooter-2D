using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagType : Enemy
{

    [SerializeField]
    private Transform[] _wayPointsToFollow;
    [SerializeField]
    private GameObject _wayPointPrefab;
    private int _currentWayPoint = 0;
    private GameObject _wayPoint;

    protected override void Start()
    {
        base.Start();
        _wayPoint = Instantiate(_wayPointPrefab, transform.position, Quaternion.identity);

        _wayPointsToFollow = new Transform[_wayPoint.transform.childCount];
        for (int i = 0; i < _wayPoint.transform.childCount; i++)
        {
            _wayPointsToFollow[i] = _wayPoint.transform.GetChild(i);
        }
    }

    protected override void CalculateMovement()
    {
        var direction = _wayPointsToFollow[_currentWayPoint].position - transform.position;
        transform.localRotation = Quaternion.LookRotation(Vector3.forward, -direction);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    protected override void Firing()
    {
        if (Time.time > _canfire && !_isDestroyed)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;

            for (int fireAngle = 0; fireAngle < 360; fireAngle += 30)
            {
                var newBullet = Instantiate(_enemyAmmoPrefab, transform.position, Quaternion.identity);
                newBullet.transform.eulerAngles = Vector3.forward * fireAngle;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
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
    }

    public override void OnHit()
    {
        if (_enemyShieldActive == false)
        {
            Debug.Log("This was called to soon?");
            Destroy(_wayPoint, 2.37f);
        }
        
        base.OnHit();
    }
}
