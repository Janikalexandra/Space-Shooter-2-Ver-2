using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCheck : MonoBehaviour
{

    private Enemy _enemy;  

    [SerializeField]
    private bool _isSmartEnemy;

    [SerializeField]
    private bool _isRammingEnemy;

    private RammingEnemy _ramEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponentInParent<Enemy>();

        if(_isRammingEnemy == true)
        _ramEnemy = gameObject.GetComponentInParent<RammingEnemy>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerLaser") && _isSmartEnemy == true)
        {
            _enemy.AvoidLaser();
        }

        if(other.gameObject.CompareTag("Player") && _isRammingEnemy)
        {
            _ramEnemy.ramPlayer = true;          
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            _enemy.ShootLaser();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _isRammingEnemy)
        {
            _ramEnemy.ramPlayer = false;
        }
    }

}
