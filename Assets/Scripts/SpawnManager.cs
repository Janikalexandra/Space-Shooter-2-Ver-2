using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemy;

    [SerializeField] 
    private GameObject _powerUps;

    [SerializeField]
    private GameObject _rarePowerUp;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerups;

    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private float chanceSpawnRare = 0.1f;

    [SerializeField]
    private GameObject[] _spawnArray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            GameObject newEnemy = Instantiate(_enemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        //yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            if(Random.Range(0f, 1f) > chanceSpawnRare)
            {
                int randomPowerUp = Random.Range(0, 6);
                Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
            else
            {
                Instantiate(_rarePowerUp, posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(3.0f);
        }       
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
