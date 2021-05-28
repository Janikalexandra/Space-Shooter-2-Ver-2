using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider slider;
    
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private int bossHealth = 20;

    public bool bossIsDead;

    private void Start()
    {
        bossIsDead = false;              
    }

    private void Update()
    {
        if(bossHealth == 0)
        {
            bossIsDead = true;
        }
       
    }

    public void SetBossMaxHealth(int bossHealth)
    {
        slider.maxValue = bossHealth;
        slider.value = bossHealth;
    }

    public void SetBossHealth(int bossHealth)
    {
        slider.value = bossHealth;
    }

    public void MinusFromBossHealth()
    {
        bossHealth -= 1;
        slider.value = bossHealth;
    }

}
