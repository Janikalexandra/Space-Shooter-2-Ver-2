using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{

    [SerializeField] private float _speed = 3f;

    [SerializeField] private float _powerupSpeed = 7f;

    private Player _player;

    private Vector2 _playerPos;
    private Vector2 position;

    [SerializeField] // 0 = Triple Shot 1 = Speed 2 = Shields
    private int powerupID;

    [SerializeField] private AudioClip _clip;  


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = _player.transform.position;

        position = gameObject.transform.position;
    }


    // Update is called once per frame
    void Update()
    {       
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
      
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player player = other.transform.GetComponent<Player>();

                AudioSource.PlayClipAtPoint(_clip, transform.position);
            
                if (player != null)
                {             
                    switch (powerupID)
                    {
                        case 0:
                        player.TripleShotActive();
                        break;

                        //case 1:
                        //player.SpeedBoostActive();
                        //break;

                        case 2:
                        player.ShieldsActive();
                        break;

                        case 3:
                        player.AmmoPickUp();
                        break;

                        case 4:
                        player.HealthPickup();
                        break;
                    
                        case 5:
                        player.MissilePickup();
                        break;

                        default:
                        Debug.Log("Default Value");
                        break;
                    }

                    Destroy(this.gameObject);
                }         
            }

            if(other.gameObject.CompareTag("EnemyLaser"))
            {
                Destroy(this.gameObject);
            }

        }

    // Go to players position
    private void FindPlayer()
    {
        float movement = _powerupSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _playerPos, movement);       
    }
   
    public void PlayerClose()
    {       
        FindPlayer();
    }

}