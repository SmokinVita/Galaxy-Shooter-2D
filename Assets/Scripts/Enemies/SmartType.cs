using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartType : Enemy
{
    //Checks to see if player is above enemy, fire backwards

    protected override void Firing()
    {
        if(transform.position.y < player.transform.position.y)
        {
            if (Time.time > _canfire && !_isDestroyed)
            {
                _fireRate = Random.Range(2f, 4f);
                _canfire = Time.time + _fireRate;

                GameObject enemyLaser = Instantiate(_enemyAmmoPrefab, transform.position + new Vector3(0, 3.26f, 0), Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                    lasers[i].AssignFiringUpwards();
                }
            }
        }

        base.Firing();
    }
    //if powerup is in front of enemy fire to destroy.
}
