using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagEnemy : MonoBehaviour
{
    [SerializeField]
    public float _speed = 4f;

    [SerializeField]
    private float frequency = 9f;

    [SerializeField]
    private float magnitude = 0.1f;

    [SerializeField]
    private GameObject _missile;

    [SerializeField]
    private GameObject _explosion;

    private float _fireRate = 3.0f;
    private float _canFire = -3f;

    private bool _isAlive = true;

    private AudioSource _audio;

    private Player _player;

    //Vector3 pos;
    Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        //pos = transform.position;
        axis = transform.right;

        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        ShootMissile();

    }

    void CalculateMovement()
    {
      //pos += Vector3.down * Time.deltaTime * _speed;
        
        
        transform.position += Vector3.down * Time.deltaTime * _speed;

        transform.position = transform.position + axis * Mathf.Sin(Time.time * frequency) * magnitude;

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8.0f);
        }
    }

    void DestroyThisGameObject()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _speed = 0f;
        _audio.Play();
        _isAlive = false;
        Destroy(this.gameObject);
    }

    private void ShootMissile()
    {
        if(_player.isAlive == true)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_missile, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            DestroyThisGameObject();
        }

        if(other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            DestroyThisGameObject();
        }
    }
}
