using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBomb : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7.8f || transform.position.y < -5.6f || transform.position.x < -11.28f || transform.position.x > 11.28f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
    }
}
