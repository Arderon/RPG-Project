using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Atrributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenHit = 1f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] Transform hips = null;
        [SerializeField] Weapon defaultWeapon = null;

        private Weapon currentWeapon;
        private GameObject currentWeaponObject;
        private float timeSinceLastHit;
        private Health target;
        private Mover mover;
        private Animator animator;

        private void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            timeSinceLastHit = timeBetweenHit;

            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            if (target == null) return;
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (target.GetComponent<Health>().IsDead())
                {
                    Cancel();
                    return;
                }
            }

            if (target.GetComponent<Health>().IsDead()) return;

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
            if (!isInRange)
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Stop();
                AttackBehaviour();
            }

            timeSinceLastHit += Time.deltaTime;
        }

        public void Atack(GameObject combatTarget)
        {
            GetComponent<ActionSceduler>().StartAction(this);
            animator.ResetTrigger("stopAttack");
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);
            if (timeSinceLastHit >= timeBetweenHit)
            {
                animator.SetTrigger("attack");
                timeSinceLastHit = 0;
            }
        }

        public bool CanAttack(GameObject enemy)
        {
            if (enemy.GetComponent<Health>().IsDead())
            {
                return false;
            }
            return true;
        }

        //Animation event
        private void Hit()
        {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        //Animation event
        private void Shoot()
        {
            Hit();
        }

        //Animation event
        private void FootL()
        {

        }

        //Animation event
        private void FootR()
        {

        }

        public void EquipWeapon(Weapon weapon)
        {
            Destroy(currentWeaponObject);
            currentWeaponObject = weapon.SpawnWeapon(rightHand, leftHand, GetComponent<Animator>());
            currentWeapon = weapon;        
        }

        public Transform GetHips()
        {
            return hips;
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }

}
