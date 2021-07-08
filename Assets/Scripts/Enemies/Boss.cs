using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    //different states depending on Health.
    //100% slow attacking, 50% quicker attack, 10% Enrage mode
    //movement will follow waypoints. In Enrage mode randomly go to different waypoints.
    //Regular enemy fire, Fire mines with scatterbomb, and Use of heat seaking missile.

    enum CurrentState { Calm, Angry, Enraged, Dead }
    [SerializeField]
    private CurrentState _currentState;

    [SerializeField]
    private GameObject _waypointContainer;
    [SerializeField]
    private Transform _startWaypoint;
    [SerializeField]
    private Transform[] _waypoints;
    [SerializeField]
    private int _currentWaypointTarget;
    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private int _health = 100;

    private bool _isMovingToStartPos = true;

    private bool _canMove = false;
    [SerializeField]
    private float _wayPointStartTime = -1f;
    [SerializeField]
    private float _wayPointCoolDownTime = 5f;

    void Start()
    {
        _waypointContainer.SetActive(false);
    }

    void Update()
    {
        //CheckState();

        if (_isMovingToStartPos == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _startWaypoint.position, _speed * Time.deltaTime);
            if (transform.position == _startWaypoint.position)
            {
                _isMovingToStartPos = false;
            }
        }

        switch (_currentState)
        {
            case CurrentState.Calm:
                break;

            case CurrentState.Angry:
                if (Time.time > _wayPointStartTime)
                {
                    AngryMovement();
                    if(transform.position == _startWaypoint.position)
                    {
                        _currentWaypointTarget += 1;
                        _wayPointStartTime = Time.time + _wayPointCoolDownTime;
                    }
                }
                break;

            case CurrentState.Enraged:
                _speed *= 2;
                break;

            case CurrentState.Dead:
                break;
        }


    }

    private void AngryMovement()
    {
        _waypointContainer.SetActive(true);

        if (_canMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypointTarget].position, _speed * Time.deltaTime);
        }
        else if (_canMove == false)
        {
            StartCoroutine(MovementRoutine());
        }
    }

    private IEnumerator MovementRoutine()
    {
        yield return new WaitForSeconds(2f);
        _canMove = true;
    }

    private IEnumerator MoveToStartRoutine()
    {
        yield return new WaitForSeconds(2f);
        _isMovingToStartPos = true;
    }

    private void CheckState()
    {
        if (_health > 50)
        {
            _currentState = CurrentState.Calm;
        }
        else if (_health < 51 && _health > 10)
        {
            _currentState = CurrentState.Angry;
        }
        else if (_health < 11 && _health > 0)
        {
            _currentState = CurrentState.Enraged;
        }
        else if (_health < 1)
        {
            _health = 0;
            _currentState = CurrentState.Dead;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _health--;
            }
        }

        if (collision.CompareTag("Laser"))
        {
            _health--;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BossWaypoint") && _isMovingToStartPos == false)
        {
            if (transform.position == _waypoints[_currentWaypointTarget].position)
            {
                _currentWaypointTarget += 1;
                _canMove = false;
                
                if (_currentWaypointTarget >= _waypoints.Length)
                {
                    _currentWaypointTarget = 0;
                }
                StartCoroutine(MovementRoutine());
            }
        }
    }
}
