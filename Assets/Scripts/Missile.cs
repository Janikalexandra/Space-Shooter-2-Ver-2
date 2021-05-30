using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Target targetEnemy;
    private SidewayEnemy sidewayEnemy;
    private ZigzagEnemy zigzagEnemy;

    [SerializeField] private Transform target;
    
    [SerializeField] private float _movementSpeed;   

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        /*if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            target = enemy.transform;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {        
            if (GameObject.FindGameObjectWithTag("Enemy") != null)
            {
            targetEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Target>();
            target = targetEnemy.transform;
            transform.right = target.position - transform.position;
                transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
            }

            if (GameObject.FindGameObjectWithTag("Enemy") != null)
            {
                targetEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Target>();
                target = targetEnemy.transform;
                transform.right = target.position - transform.position;
                transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
            }

        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            targetEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Target>();
            target = targetEnemy.transform;
            transform.right = target.position - transform.position;
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        }
    }      
}
