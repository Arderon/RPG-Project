using RPG.Atrributes;
using RPG.combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] GameObject projectilePrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride;
        [SerializeField] float damage = 15f;
        [SerializeField] float range = 2f;
        [SerializeField] bool isRightHanded = true;

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

        public float GetRange() 
        {
            return range;
        }

        public GameObject GetProjectile()
        {
            return projectilePrefab;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Transform currentHand = CurrentHand(rightHand, leftHand);
            GameObject projectile = Instantiate(projectilePrefab, currentHand.position, currentHand.rotation);
            projectile.GetComponent<Projectile>().SetTarget(target, instigator, damage);
        }

        public bool HasProjectile()
        {
            return GetProjectile();
        }
    }
}