using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    public bool canUseThrusters = true;
    public bool isAlive = true;

    [Header("Laser Settings")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _laserSpawn;
    [SerializeField] private int _ammoPickUp = 3;
    [SerializeField] private int _ammoAmount = 15;


    [Header("Triple Shot")]
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _tripleShotSpawn;
    [SerializeField] private int _tripleShotAmmo = 3;
    [SerializeField] private bool _tripleShotActive = false;

    [Header("Shield")]
    [SerializeField] private GameObject _shield;
    [SerializeField] private int _shieldLives = 0;
    [SerializeField] private bool _shieldActive = false;

    [Header("Missile")]
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private bool _missileActive = false;

    [Header("Health")]
    [SerializeField] private int _healthPickup = 1;
    public HealthBar healthBar;

    [Header("Traps")]
    [SerializeField] private bool _slowDownActive = false;
    [SerializeField] private GameObject _sparkPrefab;
    [SerializeField] private float _trapSpeed = 3f;


    //[SerializeField] private float _powerupSpeed = 2f;    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _destroyedClip;
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _hurtClip;
    private AudioSource _playerAudioSource;

    private UIManager ui_Manager;

    private WaveSpawner _waveSpawner;

    

    private CameraShake _cameraShake;

    private Thrusters _thrusters;

    // Start is called before the first frame update
    void Start()
    {
        _playerAudioSource = GetComponent<AudioSource>();

        ui_Manager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        _thrusters = GameObject.Find("Canvas").GetComponentInChildren<Thrusters>();

        healthBar = GameObject.FindGameObjectWithTag("ShieldHealth").GetComponent<HealthBar>();
        healthBar.SetHealth(0);

        ui_Manager.UpdateAmmo(15);
       
        if(_playerAudioSource == null)
        {
            Debug.LogError("Audio Source on the player is null!");
        }
        else
        {
            _playerAudioSource.clip = _laserSoundClip;
        }

        if(_thrusters == null)
        {
            Debug.LogError("Thrusters Bar is null!");
        }

        if(ui_Manager == null)
        {
            Debug.LogError("UI Manager is null!");
        }

        if(_waveSpawner == null)
        {
            Debug.LogError("Wave Spawner is null!");
        }

        _shield.SetActive(false);
        _sparkPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        //Thrusters();
        // Framework Thruster

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }

        CheckThruster();
    }

    void CheckThruster()
    {
        if (Input.GetKey(KeyCode.LeftShift) && canUseThrusters == true && _slowDownActive == false)
        {
            _normalSpeed = 10f;
            _thrusters.UseThrusters(0.3f);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            //_thrusters.RegenerateFuel(0.2f);
            _normalSpeed = 5f;
        }
        else
        {
            _thrusters.RegenerateFuel(0.2f);
        }
    }

    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _normalSpeed * Time.deltaTime);


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }


    void ShootLaser()
    {
        //Ammo Count
        if(_ammoAmount > 0)
        {
            _canFire = Time.time + _fireRate;

            if (_ammoAmount >= 3 && _tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _ammoAmount -= _tripleShotAmmo;
            } 
            else if (_missileActive == true)
            {
                Instantiate(_missilePrefab, transform.position, Quaternion.identity);
                _missileActive = false;
                _ammoAmount--;
            }
            else
            {   
                Instantiate(_laserPrefab, _laserSpawn.transform.position, Quaternion.identity);
                _ammoAmount--;
            }

            ui_Manager.UpdateAmmo(_ammoAmount);
            _playerAudioSource.Play();
        }       
    }

    //Pickup more ammo
    public void AmmoPickUp()
    {

        if(_ammoAmount <= 12)
        {          
            _ammoAmount += _ammoPickUp;
        }
        else if(_ammoAmount == 13)
        {
            _ammoAmount += 2;
        }
        else if(_ammoAmount == 14)
        {
            _ammoAmount += 1;
        }
        else
        {         
            _ammoAmount = 15;
        }

        ui_Manager.UpdateAmmo(_ammoAmount);
    }

    public void MissilePickup()
    {
        _missileActive = true;
        _ammoAmount++;
        ui_Manager.UpdateAmmo(_ammoAmount);
    }

    public void HealthPickup()
    {
        /*if(_lives <= 2)
        {
            _lives += _healthPickup;
        }*/

        if(_lives == 1)
        {
            _lives += _healthPickup;
            _leftEngine.SetActive(false);
        }
        else if(_lives == 2)
        {
            _lives += _healthPickup;
            _rightEngine.SetActive(false);
        }

        ui_Manager.UpdateLives(_lives);
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void SlowDownTrap()
    {
        _normalSpeed -= 3f;
        _sparkPrefab.SetActive(true);
        _slowDownActive = true;
        StartCoroutine(SlowDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(5f);
        _normalSpeed += _trapSpeed;
        _slowDownActive = false;
        _sparkPrefab.SetActive(false);
    }


    public void Damage()
    {
        if (_shieldActive == true)
        {
            //Shield Strength
            if (_shieldLives == 3)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(2);
                return;
            }

            if (_shieldLives == 2)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(1);
                return;
            }

            if (_shieldLives == 1)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(0);
                _shieldActive = false;
                _shield.SetActive(false);
                return;
            }
        }

        _playerAudioSource.clip = _hurtClip;
        _playerAudioSource.Play();
        StartCoroutine(_cameraShake.CameraIsShaking());
        _lives -= 1;
        

            // Enable right engine smoke
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
            // Enabel left engine smoke
        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

            ui_Manager.UpdateLives(_lives);

        if (_lives < 1)
        {
            isAlive = false;
            _waveSpawner.OnPlayerDeath();
            Destroy(this.gameObject);
        }
 }


    /*public void SpeedBoostActive()
    {
        _speedBoostEnabled = true;
        _normalSpeed *= _powerupSpeed;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostEnabled = false;
        _normalSpeed /= _powerupSpeed;
    }*/

    public void ShieldsActive()
    {
        _shieldLives = 3;
        healthBar.SetHealth(3);
        _shield.SetActive(true);
        _shieldActive = true;
    }

    public void AddScore(int points)
    {
        _score += points;
        ui_Manager.UpdateScore(_score);
    }


}