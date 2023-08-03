using RPG.Atrributes;
using RPG.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.HUD
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerHPText;
        [SerializeField] TextMeshProUGUI targetHPText;

        private Health playerHealth;
        private Health target;
        private Health previousTarget;
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
                }
                return;
            }
            targetHPText.text = target.GetPercentage().ToString() + "%";
            previousTarget = target;
        }
    }
}
