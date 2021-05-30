using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{

    private Player player;

    [SerializeField] private Transform target;

    [SerializeField] private float _movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MissileMovement();
    }

    private void MissileMovement()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        target = player.transform;
        transform.right = target.position - transform.position;
        transform.Translate(Vector3.right * _movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

    }

}
