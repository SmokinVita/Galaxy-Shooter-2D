using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3;
    [SerializeField]//0 = Triple Shot, 1 = Speed, 2 = Shield
    private int _powerupID;
    [SerializeField]
    public int _spawnWeight;

    [SerializeField]
    private AudioClip _powerUpAudio;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.88f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_powerUpAudio, transform.position + new Vector3(0, 0, -10), 1f);

                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;
                    case 1:
                        player.SpeedBoostActivate();
                        break;
                    case 2:
                        player.ShieldActivate();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.Heal();
                        break;
                    case 5:
                        player.MissileActive();
                        break;
                    case 6:
                        player.NegitivePowerup();
                        break;
                    case 7:
                        player.HomingMissileActive();
                        break;
                    default:
                        Debug.Log("No powerup selected!");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
