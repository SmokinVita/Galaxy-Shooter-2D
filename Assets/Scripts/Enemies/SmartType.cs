using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartType : Enemy
{

    [SerializeField]
    private GameObject _rayCastOrigin;

    protected override void Firing()
    {

        RaycastHit2D hit = Physics2D.Raycast(_rayCastOrigin.transform.position, Vector2.down, 2f);
        Debug.DrawRay(_rayCastOrigin.transform.position, Vector2.down * 2f, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Powerup"))
            {
                _canfire = 1;
            }
        }

        if (player != null)
        {
            if (transform.position.y < player.transform.position.y)
            {
                FireUptowardsPlayer();
            }
        }

        base.Firing();
    }

    private void FireUptowardsPlayer()
    {
        if (Time.time > _canfire && !_isDestroyed)
        {
            _fireRate = Random.Range(2f, 4f);
            _canfire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_enemyAmmoPrefab, transform.position + new Vector3(0, 1.27f, 0), Quaternion.identity);
            PowerupDestroyer powerupDestroyer = enemyLaser.GetComponent<PowerupDestroyer>();

            powerupDestroyer.AssignFiringUpwards();
        }
    }
}
