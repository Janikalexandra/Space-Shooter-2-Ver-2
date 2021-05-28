using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Enemy enemy;
    private SidewayEnemy sidewayEnemy;

    [SerializeField] private Transform target;
    
    [SerializeField] private float _movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        target = enemy.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
            target = enemy.transform;
            transform.right = target.position - transform.position;
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        }    
        
        if(GameObject.FindGameObjectWithTag("SidewayEnemy") != null)
        {
            sidewayEnemy = GameObject.FindGameObjectWithTag("SidewayEnemy").GetComponent<SidewayEnemy>();
            target = sidewayEnemy.transform;
            transform.right = target.position - transform.position;
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        }
        
    }

}
