using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalTower : Entity
{
    public Canvas healthbarCanvas;
    public Image healthbar;
    public Image gameOverImage;
    public float delay = 5.0f;
    public string sceneName;


    void Start()
    {
        hp = maxHP;
    }

    void Update()
    {
        if (hp <= 0)
        {
            // GameOver
            GameManager.Instance.TogglePause();
            gameOverImage.gameObject.SetActive(true);
            StartCoroutine(DelayToSceneSwitch());
        }
    }

    IEnumerator DelayToSceneSwitch()
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
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
