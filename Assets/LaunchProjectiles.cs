using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectiles : MonoBehaviour
{
    [SerializeField] private int _numberOfProjectiles;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private GameObject _projectileContainer;

    [SerializeField] private Transform _spawn;
    
    Vector2 startPoint;

    float radius, moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //startPoint = _spawn.transform.position;
        radius = 5f;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnProjectiles(int numberOfProjectiles)
    {
        startPoint = _spawn.transform.position;

        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for(int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;

            var proj = Instantiate(_projectile, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;           
        }
    }
}
