using RPG.Atrributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RPG.Stats;

namespace RPG.HUD
{
    public class ExperianceDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI experianceValueText;

        private Experiance playerExperiance;

        void Awake()
        {
            playerExperiance = GameObject.FindWithTag("Player").GetComponent<Experiance>();
        }

        private void Update()
        {
            experianceValueText.text = playerExperiance.GetExperiance().ToString();
        }
    }
}