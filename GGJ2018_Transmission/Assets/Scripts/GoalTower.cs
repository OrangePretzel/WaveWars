using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTower : Entity
{
    public Canvas healthbarCanvas;
    public Image healthbar;

    void Start()
    {
        
    }

    void Update()
    {
        if (hp <= 0)
        {
            // GameOver
        }
    }

    public override void UpdateHealthBar()
    {
        if (maxHP > hp)
        {
            healthbarCanvas.enabled = true;
            healthbar.fillAmount = hp / maxHP;
        }
        else
        {
            healthbarCanvas.enabled = false;
            healthbar.fillAmount = 1;
        }
    }

}
