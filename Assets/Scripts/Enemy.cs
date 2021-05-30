using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Universal Enemy Settings")]
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField] private AudioClip _destroyedClip;
    [SerializeField] private AudioClip _shieldDestroyed;
    [SerializeField] private AudioSource _audio;


    [Header("Normal Enemy Settings")]

    [SerializeField]
    private bool _normalEnemy = false;

    [SerializeField]
    private GameObject _enemyShield;

    [SerializeField]
    private bool _shieldActive = false;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    public bool _isAlive = true;


    [SerializeField]
    private GameObject _laserPrefab;

    [Header("Smart Enemy Settings")]
    [SerializeField]
    private bool _isSmartEnemy;

    [SerializeField]
    private bool hasDodged;

    [SerializeField]
    private GameObject _smartLaserPrefab;

    [SerializeField]
    private GameObject _backwardLaserSpawn;

    [SerializeField]
    private float avoidAmount;

    [SerializeField]
    private GameObject _playerCheck;

    [SerializeField]
    private GameObject _laserCheck;

    private Player _player;

    private Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        if(_normalEnemy == true)
        {
            _enemyShield.SetActive(false);
        }       

        hasDodged = false;

        _audio = GetComponent<AudioSource>();

        if(_audio == null)
        {
            Debug.LogError("Enemy Audio Source is null!");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        if(_player == null)
        {
            Debug.LogError("Player is null!");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is null!");
        }

        if(_normalEnemy == true)
        {
            //40% chance to spawn with a shield
            if(Random.value > 0.6)
            CalculateShieldChance();
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        EnemyShoot();
    
    }

    private void CalculateShieldChance()
    {
        _shieldActive = true;
        _enemyShield.SetActive(true);
    }

    private void EnemyShoot()
    {
        if (Time.time > _canFire && _isAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }

    public void RamTowardsPlayer()
    {
        _speed *= 2;
    }

    //Enemy avoids laser
    public void AvoidLaser()
    {
        if(hasDodged == false)
        {
            hasDodged = true;
            transform.Translate(avoidAmount,0,0);
            StartCoroutine(GoBack());
        }
    }

    public void ShootBackwardsLaser()
    {
        Instantiate(_smartLaserPrefab, _backwardLaserSpawn.transform.position, Quaternion.identity);
    }

    public void ShootLaser()
    {
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);       
    }

    void CalculateMovement()
    {    
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {                   
            if(_player != null)
            {
                _player.Damage();
            }

            if(_shieldActive == true)
            {
                _audio.clip = _shieldDestroyed;
                _audio.Play();
                _shieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            else
            {
                DestroyThisGameObject();
            }
            
            if(_isSmartEnemy == true)
            {
                Destroy(_playerCheck);
            }

        }

        if (other.tag == "PlayerLaser" || other.tag == "Missile" || other.tag == "Smart Enemy Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }

            if(_shieldActive == true)
            {
                _audio.clip = _shieldDestroyed;
                _audio.Play();
                _shieldActive = false;
                _enemyShield.SetActive(false);
                return;
            }
            else
            {
                DestroyThisGameObject();

                if (_isSmartEnemy == true)
                {
                    Destroy(_playerCheck);
                }
            }          
        }
    }

    private IEnumerator GoBack()
    {
        yield return new WaitForSeconds(2f);
        transform.Translate(-2, 0, 0);
    } 

    
}
