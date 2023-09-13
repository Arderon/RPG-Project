using RPG.Saving;
using UnityEngine;
using RPG.Stats;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Events;

namespace RPG.Atrributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent playerDie;
        float health;
        bool isDead = false;


        private void Awake()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            Experiance experiance = GetComponent<Experiance>();
            if (experiance != null)
            {
                experiance.onLevelUp += RestoreHealth;
            }
        }

        public void RestoreHealth()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
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
            GetComponent<Collider>().enabled = false;
        }

        private void AwardExperiance(GameObject instigator)
        {
            float xpReward = gameObject.GetComponent<BaseStats>().GetStat(Stat.ExperianceReward);
            if (instigator.GetComponent<Experiance>() == null) return;
            instigator.GetComponent<Experiance>().GainExperiance(xpReward);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);
            takeDamage.Invoke(damage);
            if (health == 0)
            {
                Die(instigator);
                if(gameObject.tag == "Player")
                {
                    playerDie.Invoke();
                }
                AwardExperiance(instigator);
            }
        }

        public float GetHealth()
        {
            return health;
        }

        public float GetPercentage()
        {
            return Mathf.Round(100 * (health / GetComponent<BaseStats>().GetStat(Stat.Health)));
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