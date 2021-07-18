
using System;
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum EnemyCurentState
    {
        Idle,
        Moving,
        Attacking,
        LaserAttack,
        Dead
    }

    [SerializeField]
    private EnemyCurentState _currentState;

    private Animator anim;

    [SerializeField]
    private int _health = 100;
    private bool _isRageMode = false;

    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private Transform _rightTurrent;
    [SerializeField]
    private Transform _leftTurrent;
    [SerializeField]
    private GameObject _scatterBomb;
    private bool canFire;
    [SerializeField]
    private float _turrentFireTime = 1.5f;
    [SerializeField]
    private float _scatterFireTime = 1f;

    [SerializeField]
    private GameObject _explosion;

    private bool _isIdle = false;
    private bool _isInvincible = false;

    [SerializeField]
    private int _playerShipDmg = 2;
    [SerializeField]
    private int _laserDmg = 1;
    [SerializeField]
    private int _heatSeekingDmg = 10;
    [SerializeField]
    private int _homingMissileDmg = 20;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _currentState = EnemyCurentState.Idle;
    }

    private void Update()
    {

        if (_health <= 50)
        {
            anim.SetBool("IsRageMode", true);
            _isRageMode = true;
        }

        CheckState();
    }

    private void CheckState()
    {
        switch (_currentState)
        {
            case EnemyCurentState.Idle:
                if (_isIdle == true)
                    StartCoroutine(IdlePostionRoutine());
                break;
            case EnemyCurentState.Moving:
                Movement();
                break;
            case EnemyCurentState.Attacking:
                if (!_isRageMode)
                {
                    BasicFiring();
                }
                else
                {
                    ScatterFire();
                }
                break;
            case EnemyCurentState.LaserAttack:
                anim.SetTrigger("Laser");
                break;
            case EnemyCurentState.Dead:

                break;
        }
    }

    //While in Idle Position every few seconds decide weather to move or fire.
    private IEnumerator IdlePostionRoutine()
    {
        _isIdle = false;
        yield return new WaitForSeconds(2f);
        int randState = UnityEngine.Random.Range(0, 4);
        switch (randState)
        {
            case 0://Return to Idle state
                SetState(EnemyCurentState.Idle);
                SetToIdle();
                break;
            case 1://move
                SetState(EnemyCurentState.Moving);
                break;
            case 2://Fire
                SetState(EnemyCurentState.Attacking);
                yield return new WaitForSeconds(2f);
                SetState(EnemyCurentState.Idle);
                SetToIdle();
                break;
            case 3://Fire Laser
                if(_isRageMode)
                {
                    SetState(EnemyCurentState.LaserAttack);
                }else
                {
                    SetState(EnemyCurentState.Idle);
                    SetToIdle();
                }
                break;
        }
    }

    public void SetToIdle()
    {
        _isIdle = true;
    }

    public void SetToInvincible()
    {
        _isInvincible = true;
    }

    public void SetToVulnerable()
    {
        _isInvincible = false;
    }

    private void Damage(int dmgAmount)
    {
        if (_isInvincible)
            return;
        _health -= dmgAmount;

        if (_health <= 0)
        {
            _currentState = EnemyCurentState.Dead;
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject, .5f);
        }
    }

    public void SetState(EnemyCurentState enemyCurentState)
    {
        _currentState = enemyCurentState;
        if (enemyCurentState == EnemyCurentState.Attacking)
            canFire = true;
        else
            StopAllCoroutines();
    }

    public void ResetTriggers()
    {
        foreach (var animPram in anim.parameters)
        {
            if (animPram.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(animPram.name);
            }
        }
    }

    private void BasicFiring()
    {
        if (canFire == true)
        {
            StartCoroutine(BasicfireRoutine());
            canFire = false;
        }
    }

    private IEnumerator BasicfireRoutine()
    {
        while (true)
        {
            GameObject bossRightLaser = Instantiate(_enemyLaser, _rightTurrent.position, Quaternion.identity);
            Laser rightLaser = bossRightLaser.GetComponentInChildren<Laser>();
            rightLaser.AssignEnemyLaser();
            yield return new WaitForSeconds(_turrentFireTime);
            GameObject bossLeftLaser = Instantiate(_enemyLaser, _leftTurrent.position, Quaternion.identity);
            Laser leftLaser = bossLeftLaser.GetComponentInChildren<Laser>();
            leftLaser.AssignEnemyLaser();
            yield return new WaitForSeconds(_turrentFireTime);
        }
    }

    private void ScatterFire()
    {
        if (canFire == true)
        {
            StartCoroutine(ScatterFireRoutine());
            canFire = false;
        }
    }

    private IEnumerator ScatterFireRoutine()
    {
        for (int fireAngle = 0; fireAngle < 360; fireAngle += 30)
        {
            var newBullet = Instantiate(_scatterBomb, transform.position, Quaternion.identity);
            newBullet.transform.eulerAngles = Vector3.forward * fireAngle;
        }
        yield return new WaitForSeconds(_scatterFireTime);
        canFire = true;
    }

    private void Movement()
    {
        if (!_isRageMode)
        {
            int randMove = UnityEngine.Random.Range(0, 2);
            if (randMove == 0)
            {
                anim.SetTrigger("MoveLeft");
            }
            else if (randMove == 1)
            {
                anim.SetTrigger("MoveRight");
            }
        }
        else
        {
            anim.SetTrigger("RageMove");
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
                Damage(_playerShipDmg);
            }
        }

        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            Damage(_laserDmg);
        }

        if (collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
            Damage(_heatSeekingDmg);
        }

        if (collision.CompareTag("Homing Missile"))
        {
            Destroy(collision.gameObject);
            Damage(_homingMissileDmg);
        }
    }
}
