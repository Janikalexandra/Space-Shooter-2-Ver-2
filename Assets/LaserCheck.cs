using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCheck : MonoBehaviour
{

    private Enemy _enemy;

    [SerializeField]
    private bool _isSmartEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerLaser") && _isSmartEnemy == true)
        {
            _enemy.AvoidLaser();
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            _enemy.ShootLaser();
        }
    }

}
