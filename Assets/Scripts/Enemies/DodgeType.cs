using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeType : Enemy
{

    protected override void CalculateMovement()
    {
        Collider2D laser = Physics2D.OverlapBox(transform.position + new Vector3(0, -2f), new Vector2(1, 2), 0);
        if (laser != null && _isDestroyed == false)
        {
            if (laser.CompareTag("Laser"))
            {
                PickDirection();
            }
        }

        base.CalculateMovement();
    }

    private void PickDirection()
    {
        float randomNumber = UnityEngine.Random.Range(1, 6);
        switch (randomNumber)
        {
            case 1:
                transform.position = new Vector3(transform.position.x + 2, transform.position.y);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x - 2, transform.position.y);
                break;
            default:
                break;  
        }
    }
}
