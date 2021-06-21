using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeType : Enemy
{

    [SerializeField]
    private float _radius = .5f;

    protected override void CalculateMovement()
    {
        Collider2D laser = Physics2D.OverlapCapsule(transform.position, Vector2.one, CapsuleDirection2D.Vertical, 90f);
        if (laser.CompareTag("Laser"))
        {
            Debug.Log("Found Laser!");
        }
        base.CalculateMovement();
    }

   
}
