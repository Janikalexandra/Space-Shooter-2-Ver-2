using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour
{

    private Powerup _powerup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Powerup")
        {
            if (Input.GetKey(KeyCode.C))
            {
                _powerup = other.transform.GetComponent<Powerup>();
                _powerup.PlayerClose();
            }
        }      
    }

}
