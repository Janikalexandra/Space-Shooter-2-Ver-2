using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public enum BossState { START, WAITING, MOVING, SHOOT, VULNERABLE}

    [SerializeField]
    private float _speed;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private Rigidbody2D rb2;
    private Vector3 _endPosition = new Vector3(-0.1f, 3.3f, 0);

    [SerializeField]
    private bool _arrivedEndPosition = false;

    [SerializeField]
    private bool _vulnerable = true;
    private bool movedRight = false;  

    [Header("Boss Positions")]
    private Vector3 _currentPos;
    private Vector3 _onePoint = new Vector3(6f, 3.3f,0);
    private Vector3 _secondPoint = new Vector3(-6, 3.3f, 0);

    private float myTime;

    [SerializeField]
    private GameObject _explosion;

    private LaunchProjectiles[] _launchProjectiles;

    private SpriteRenderer _spriteRenderer;

    private BossHealth _bossHealth;

    private Animator _anim;
   

    [SerializeField]
    private AudioClip _hurtClip;

    [SerializeField] 
    private AudioClip[] _bossClips;

    [Header("Boss Mode")]
    [SerializeField]
    private BossState state = BossState.START;

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        _currentPos = transform.position;

        _launchProjectiles = GetComponentsInChildren<LaunchProjectiles>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(VulnerableCountdown());

        _bossHealth = GameObject.Find("Canvas").GetComponentInChildren<BossHealth>();
        _bossHealth.SetBossHealth(20);

        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_bossHealth.bossIsDead == true)
        {
            BossDeathSeq();
        }


        if (state == BossState.START)
        {
            if (transform.position != _endPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, _endPosition, _speed * Time.deltaTime);
            }
            else
            {
                _arrivedEndPosition = true;
                state = BossState.WAITING;
            }
        }

        if (state == BossState.WAITING)
        {
            StartCoroutine(WaitAndMove(3));
        }

        if (state == BossState.MOVING)
        {
            if (movedRight == false)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }

        if (state == BossState.VULNERABLE)
        {
            _vulnerable = true;
            StartCoroutine(EngineBreakDown());
        }

        if (state == BossState.SHOOT)
        {
            BossShoot();
        }    
    }
    public void PlayClip(int clipNumber)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = _bossClips[clipNumber];
        audio.Play();
    }

    private IEnumerator WaitAndMove(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        state = BossState.MOVING;
    }

    private IEnumerator EngineBreakDown()
    {
        StartCoroutine(Recover());
        while (_vulnerable == true)
        {
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1, 0.240566f, 0.240566f);
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }    
    }

    public void BossDeathSeq()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);     
    }


    private void MoveRight()
    {
        if(transform.position != _onePoint)
        {
            transform.position = Vector2.MoveTowards(transform.position, _onePoint, 1f * Time.deltaTime);
        }
        else
        {        
            movedRight = true;
            state = BossState.SHOOT;
        }
    }

    void MoveLeft()
    {
        if (transform.position != _secondPoint)
        {
            transform.position = Vector2.MoveTowards(transform.position, _secondPoint, 1f * Time.deltaTime);
        }
        else
        {          
            movedRight = false;
            state = BossState.SHOOT;
        }
    }

    private IEnumerator VulnerableCountdown()
    {
        yield return new WaitForSeconds(Random.Range(10,20));
        _vulnerable = true;
        StartCoroutine(EngineBreakDown());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(7);
        _vulnerable = false;
        StartCoroutine(VulnerableCountdown());
    }

    private void BossShoot()
    {
        StartCoroutine(WaitAndMove(6));
        if (Time.time > _canFire)
        {
            PlayClip(1);
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            _launchProjectiles[0].SpawnProjectiles(5);
            _launchProjectiles[1].SpawnProjectiles(5);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_vulnerable == true && other.tag == "PlayerLaser")
        {
            _bossHealth.MinusFromBossHealth();
            PlayClip(0);
            _anim.Play("BossHurt_anim");
            Destroy(other.gameObject);
        }
    }

}
