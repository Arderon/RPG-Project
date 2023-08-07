using RPG.Combat;
using RPG.Core;
using RPG.Atrributes;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] UnityEvent ProjectileHit;
        [SerializeField] UnityEvent LaunchProjectile;
        [SerializeField] float speed = 20f;
        [SerializeField] GameObject trail;
        [SerializeField] GameObject onHitEffect;
        [SerializeField] bool homing;
        [SerializeField] bool isStuck;

        private GameObject insigator;
        private Health target;
        private float damage;
        private bool isStoped = false;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            LaunchProjectile.Invoke();
        }

        void Update()
        {
            if (target == null) {return;}
            if (isStoped ) return;
            if (!target.GetComponent<Health>().IsDead())
            {
                if (homing)
                {
                    transform.LookAt(GetAimLocation());
                }
            }
            else if (!isStoped)
            {
                StartCoroutine(DestroyCountDown(5f));
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }   

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.insigator = instigator;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.GetComponent<Health>().IsDead()) return;
            if (other.gameObject == target.gameObject)
            {
                OnHit(other);
                ProjectileHit.Invoke();
            }
        }

        private void OnHit(Collider other)
        {
            other.GetComponent<Health>().TakeDamage(insigator, damage);
            if (isStuck)
            {
                isStoped = true;
                damage = 0;
                Destroy(trail);
                transform.SetParent(other.GetComponent<Fighter>().GetHips());
                DestroyCountDown(20f);
            }
            else
            {
                Instantiate(onHitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        IEnumerator DestroyCountDown(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }
}
