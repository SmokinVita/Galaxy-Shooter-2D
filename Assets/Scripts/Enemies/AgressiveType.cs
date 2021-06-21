using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveType : Enemy
{

    [SerializeField]
    private float _specifiedDistance = 2f;
    private Quaternion _startRotation;
    // If enemy is within a distance rotate and ram. return normal path if outside of distance

    protected override void Start()
    {
        _startRotation = transform.rotation;
        base.Start();
    }

    protected override void CalculateMovement()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            if (distance <= _specifiedDistance)
            {
                Vector2 direction = player.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, -direction);
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            else if (distance >= _specifiedDistance)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _startRotation, 1);
                base.CalculateMovement();
            }
        }
    }

}
