using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

   [System.Serializable]
   public class Wave
   {
        public string name;

        public GameObject normalEnemy;
        public GameObject smartEnemy;
        public GameObject sidewayEnemy;
        public GameObject rammingEnemy;
        public GameObject Boss;

        public int normalEnemyCount;
        public int smartEnemyCount;
        public int sidewayEnemyCount;
        public int rammingEnemyCount;

        public float spawnRate;
   }
    [SerializeField]
    private bool _stopSpawning = false;

    [Header("Power Ups")]
    [SerializeField]
    private GameObject[] _rarePowerUps;

    [SerializeField]
    private GameObject _commonPowerUp;

    [SerializeField]
    private GameObject[] _ultraRarePowerUps;

    public Wave[] enemyWaves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    [SerializeField]
    private float searchCountdown = 1f;

    [SerializeField]
    private Text _waveText;

    [SerializeField]
    private Text _waveCompletedText;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _powerUpContainer;

    [SerializeField]
    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        _waveCompletedText.gameObject.SetActive(false);
    }

    private void Update()
    {

        if(state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnEnemyWave(enemyWaves[nextWave]));
                StartCoroutine(SpawnPowerUpWave());
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        _waveCompletedText.gameObject.SetActive(true);
        StartCoroutine(WaveCompleteFlicker());

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > enemyWaves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("Completed all waves!");
        }
        else
        {
            nextWave++;
        }       
    }

    public bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }      
        return true;
    }

    IEnumerator SpawnEnemyWave(Wave _wave)
    {      
        while(_stopSpawning == false)
        {
            _waveCompletedText.gameObject.SetActive(false);
            _waveText.text = "Spawning Wave: " + _wave.name.ToString();
            StartCoroutine(SetWaveText());
            state = SpawnState.SPAWNING;

            for (int i = 0; i < _wave.normalEnemyCount; i++)
            {
                SpawnEnemy(_wave.normalEnemy);             
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }

            for(int i = 0; i < _wave.smartEnemyCount; i++)
            {
                SpawnEnemy(_wave.smartEnemy);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }

            for (int i = 0; i < _wave.sidewayEnemyCount; i++)
            {
                SpawnSidewayEnemy(_wave.sidewayEnemy);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }

            for (int i = 0; i < _wave.rammingEnemyCount; i++)
            {
                SpawnEnemy(_wave.rammingEnemy);
                yield return new WaitForSeconds(1f / _wave.spawnRate);
            }

            state = SpawnState.WAITING;
            yield break;
        }
    }

    IEnumerator SpawnPowerUpWave()
    {
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
               
            Instantiate(_commonPowerUp, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(7);
            
            if(Random.Range(0f, 1f) > 0.5)
            {
                int randomRarePowerUp = Random.Range(0, 2);
                Instantiate(_rarePowerUps[randomRarePowerUp], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(9, 11));
            }
            else if(Random.Range(0f, 1f) < 0.2)
            {
                int randomUltraPowerUp = Random.Range(0, 2);
                Instantiate(_ultraRarePowerUps[randomUltraPowerUp], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(11);
            }
        }
    }


    void SpawnEnemy(GameObject _enemy)
    {
        if (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    void SpawnSidewayEnemy(GameObject _enemy)
    {
        if(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(12f, Random.Range(-3f, 3f), 0);
            GameObject newEnemy = Instantiate(_enemy, posToSpawn, Quaternion.Euler(0f,0f,90f));
            newEnemy.transform.parent = _enemyContainer.transform;
        }
    }

    /*void SpawnPowerUps(Transform _powerUps)
    {
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_powerUps, posToSpawn, Quaternion.identity);
        }
    }*/

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private IEnumerator SetWaveText()
    {
        yield return new WaitForSeconds(4);
        _waveText.text = "";
    }

    private IEnumerator WaveCompleteFlicker()
    {
        while (true)
        {
            _waveCompletedText.text = "Wave Completed";
            yield return new WaitForSeconds(0.5f);
            _waveCompletedText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
