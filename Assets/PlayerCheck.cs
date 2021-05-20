using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private Enemy _smartEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //_smartEnemy = GameObject.Find("SmartEnemy").GetComponent<Enemy>();
        _smartEnemy = gameObject.GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _smartEnemy.ShootBackwardsLaser();
        }
    }
}
