using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDestroyer : MonoBehaviour
{
    private bool _isFiringBackwards = false;
    [SerializeField]
    private float _speed = 3.5f;

    // Update is called once per frame
    void Update()
    {
        if(_isFiringBackwards)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    public void AssignFiringUpwards()
    {
        _isFiringBackwards = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Powerup"))
        {
            Debug.Log($"{gameObject.name} hit powerup!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
