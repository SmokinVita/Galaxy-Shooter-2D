using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;

    private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.6f)
        {
            float newXPos = Random.Range(-9f, 9f);
            transform.position = new Vector3(newXPos, 7.8f, 0);
        }

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }

        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);

            if (player != null)
                player.AddScore();

            Destroy(this.gameObject);
        }
    }
}
