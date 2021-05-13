using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    private Image _thrustersBar;

    [SerializeField]
    private float fuel = 1;

    private Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (_thrustersBar.fillAmount <= 0)
        {
            StartCoroutine(WaitAndThrust());
        }       
    }

    private IEnumerator WaitAndThrust()
    {
        player.canUseThrusters = false;
        yield return new WaitForSeconds(3f);
        player.canUseThrusters = true;
    }

    public void UseThrusters(float usingFuel)
    {
        if(fuel > 0)
        {
           _thrustersBar.fillAmount = fuel -= usingFuel * Time.deltaTime;
        }
    }

    public void RegenerateFuel(float regenerate)
    {
        if (fuel < 1)
        {
            _thrustersBar.fillAmount = fuel += regenerate * Time.deltaTime;
        }
    }
}
