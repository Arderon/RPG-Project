using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Atrributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent;
        [SerializeField] RectTransform healthBar;

        void Update()
        {
            float health = healthComponent.GetPercentage() / 100;
            if (health >= 1) 
            {
                HideHealthBar();
            }else if(health <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                ShowHealthBar();
            }
            healthBar.localScale = new Vector3(health, 1, 1);
        }

        private void HideHealthBar()
        {
            healthBar.transform.parent.GetComponent<Image>().enabled = false;
            healthBar.GetComponent<Image>().enabled = false;
        }

        private void ShowHealthBar()
        {
            healthBar.transform.parent.GetComponent<Image>().enabled = true;
            healthBar.GetComponent<Image>().enabled = true;
        }
    }

}