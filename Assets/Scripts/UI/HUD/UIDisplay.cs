using RPG.Atrributes;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    [SerializeField] Image xpSlider;
    [SerializeField] Image hpSlider;
    [SerializeField] Image manaSlider;
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI levelText;

    GameObject player;
    Health health;
    Experiance experiance;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
        experiance = player.GetComponent<Experiance>();
    }

    void Update()
    {
        hpSlider.fillAmount = health.GetPercentage() / 100;
        hpText.text = health.GetHealth().ToString() + "/" + player.GetComponent<BaseStats>().GetStat(Stat.Health).ToString();

        xpSlider.fillAmount = experiance.GetPercentage();
        xpText.text = experiance.GetExperiance().ToString() + "/" + player.GetComponent<BaseStats>().GetStat(Stat.ExperianceToLevelUp).ToString() + "XP";

        /*manaSlider.fillAmount = health.GetPercentage() / 100;*/

        int level = player.GetComponent<BaseStats>().GetLevel();
        if (level >= 10)
        {
            levelText.fontSize = 44;
        }
        levelText.text = level.ToString();
    }
}
