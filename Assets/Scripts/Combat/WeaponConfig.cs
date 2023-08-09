using RPG.Atrributes;
using RPG.combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] GameObject projectilePrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride;
        [SerializeField] float damage = 15f;
        [SerializeField] float range = 2f;
        [SerializeField] bool isRightHanded = true;

        private float percentageBonus = 2;

        public GameObject SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (weaponPrefab != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
                return Instantiate(weaponPrefab, CurrentHand(rightHand, leftHand));
            }
            else return null;
        }

        private Transform CurrentHand(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetRange() 
        {
            return range;
        }

        public GameObject GetProjectile()
        {
            return projectilePrefab;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Transform currentHand = CurrentHand(rightHand, leftHand);
            GameObject projectile = Instantiate(projectilePrefab, currentHand.position, currentHand.rotation);
            projectile.GetComponent<Projectile>().SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile()
        {
            return GetProjectile();
        }
    }
}