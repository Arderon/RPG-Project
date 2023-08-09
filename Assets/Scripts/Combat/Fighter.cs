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
        [SerializeField] WeaponConfig defaultWeapon = null;

        private WeaponConfig currentWeaponConfig;
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

            if(currentWeaponConfig == null)
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

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetRange();
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
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
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

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
                if (currentWeaponObject == null) return;
                currentWeaponObject.GetComponent<Weapon>().OnHit();
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

        public void EquipWeapon(WeaponConfig weapon)
        {
            Destroy(currentWeaponObject);
            currentWeaponObject = weapon.SpawnWeapon(rightHand, leftHand, GetComponent<Animator>());
            currentWeaponConfig = weapon;        
        }

        public Transform GetHips()
        {
            return hips;
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            if (currentWeaponConfig == null) return null;
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }

}
