using RPG.Atrributes;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Health playerHealth;
    private Health target;
    private Health previousTarget;
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI targetHPText;
    void Awake()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    void Update()
    {
        playerHPText.text = playerHealth.GetPercentage().ToString() + "%";

        target = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
        if (target == null)
        {
            if (previousTarget)
            {
                targetHPText.text = previousTarget.GetPercentage().ToString() + "%";
            }return;
        };
        targetHPText.text = target.GetPercentage().ToString() + "%";
        previousTarget = target;
    }
}
