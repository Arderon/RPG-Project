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

        private void Update()
        {
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
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
                Die();
            }
        }
    }
}