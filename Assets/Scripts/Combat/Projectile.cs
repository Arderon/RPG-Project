using Newtonsoft.Json;
using RPG.Combat;
using RPG.Core;
using RPG.Atrributes;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 20f;
        [SerializeField] GameObject trail;
        [SerializeField] GameObject onHitEffect;
        [SerializeField] bool homing;
        [SerializeField] bool isStuck;

        private Health target;
        private float damage;
        private bool isStoped = false;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
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
                StartCoroutine(DestroyCountDown());
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

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.GetComponent<Health>().IsDead()) return;
            if (other.gameObject == target.gameObject)
            {
                OnHit(other);
            }
        }

        private void OnHit(Collider other)
        {
            other.GetComponent<Health>().TakeDamage(damage);
            if (isStuck)
            {
                isStoped = true;
                damage = 0;
                Destroy(trail);
                transform.SetParent(other.GetComponent<Fighter>().GetHips());
            }
            else
            {
                Instantiate(onHitEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        IEnumerator DestroyCountDown()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}
