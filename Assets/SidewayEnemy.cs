using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewayEnemy : MonoBehaviour
{
    private float _speed = 3f;

    [SerializeField]
    private GameObject _explosion;

    private Player _player;

    [SerializeField]
    private AudioSource _audio;

    [SerializeField]
    private AudioClip _destroyedClip;

    public bool _isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audio = GetComponent<AudioSource>();

        if (_audio == null)
        {
            Debug.LogError("Enemy Audio Source is null!");
        }

        if (_player == null)
        {
            Debug.LogError("Player is null!");
        }

        _isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x < -12f)
        {
            float randomY = Random.Range(-3f, 3f);
            transform.position = new Vector3(12f, randomY);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(_player != null)
            {
                _player.Damage();
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _audio.Play();
                _isAlive = false;
                Destroy(this.gameObject);
            }            
        }

        if (other.tag == "PlayerLaser" || other.tag == "Missile" || other.tag == "Enemy" || other.tag == "EnemyLaser")
        {
            _player.AddScore(15);

            Instantiate(_explosion, transform.position, Quaternion.identity);
            _audio.Play();
            _isAlive = false;

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
