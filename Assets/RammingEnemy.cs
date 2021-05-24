using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private GameObject _explosion;

    public bool ramPlayer = false;

    private AudioSource _audio;

    private Player _player;

    public bool _isAlive;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        if (ramPlayer == true)
        {
            _speed = 10f;
        }
        else 
        {
            _speed = 3f;
        }

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8.0f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            
            if(_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "PlayerLaser" || other.tag == "Missile" || other.tag == "Enemy" || other.tag == "EnemyLaser")
        {
            _player.AddScore(10);

            Instantiate(_explosion, transform.position, Quaternion.identity);
            _audio.Play();
            _isAlive = false;

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
