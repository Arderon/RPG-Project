using RPG.Saving;
using UnityEngine;
using RPG.Stats;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace RPG.Atrributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100;
        bool isDead = false;


        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void Die(GameObject instigator)
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");

        }

        private void AwardExperiance(GameObject instigator)
        {
            float xpReward = gameObject.GetComponent<BaseStats>().GetExperianceReward();
            instigator.GetComponent<Experiance>().GainExperiance(xpReward);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die(instigator);
                AwardExperiance(instigator);
            }
        }

        public float GetPercentage()
        {
            return Mathf.Round(100 * (health / GetComponent<BaseStats>().GetHealth()));
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if(health <= 0)
            {
                Die(gameObject);
            }
        }
    }
}