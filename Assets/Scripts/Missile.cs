using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    private Enemy enemy;

    [SerializeField] private Transform target;
    [SerializeField] private float _movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        target = enemy.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        if(enemy._isAlive == true)
        {
            target = enemy.transform;
            transform.right = target.position - transform.position;
            transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
        }
    }

}
