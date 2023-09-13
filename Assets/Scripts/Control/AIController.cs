using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Atrributes;
using UnityEngine.UIElements;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float patrolSpeed;
        [SerializeField] float runSpeed = 3.5f;
        [SerializeField] float visionDistance = 20f;
        [SerializeField] float detectionRadius = 5f;
        [SerializeField] float suspiciosTime = 3f;
        [SerializeField] float dwellTime = 2f;
        [SerializeField] float aggrevatedTime = 5f;
        [SerializeField] float shoutRadius = 5f;
        [SerializeField] PatrolPath patrolPath;

        private GameObject player;
        private Vector3 guardingPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceStop = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;
        private int currentWaypointIndex;


        private ActionSceduler sceduler;
        private Fighter fighter;
        private Mover mover;
        private Health health;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            sceduler = GetComponent<ActionSceduler>();

            player = GameObject.FindGameObjectWithTag("Player");
            guardingPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead())
            {
                sceduler.CancelCurrentAction();
                return;
            }

            if (fighter.CanAttack(player) && IsAggrevated())
            {
                AttackBehaviour();
                
            }
            else if (timeSinceLastSawPlayer < suspiciosTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        public bool IsAggrevated()
        {
            if (DistanceToPlayer() > visionDistance) return false;
            float distanceToPlayer =  DistanceToPlayer();
            return distanceToPlayer < detectionRadius || timeSinceAggrevated < aggrevatedTime;
        }

        public void Agreevate()
        {
            timeSinceAggrevated = 0;
        }

        private void AgreevateNearbyEnemyes()
        {
            RaycastHit[] reycastHits = Physics.SphereCastAll(transform.position, shoutRadius, Vector3.up, 0);
            foreach(RaycastHit hit in reycastHits)
            {
                AIController enemy = hit.transform.GetComponent<AIController>();
                if (enemy == null) return;
                if (!enemy.IsAggrevated())
                {
                    enemy.Agreevate();
                }
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceStop += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            sceduler.CancelCurrentAction();
            Vector3 nextPosition = guardingPosition;

            if(patrolPath != null)
            {
                if (AtWayPoint())
                {
                    CycleWaypont();
                    timeSinceStop = 0;
                }
                nextPosition = GetCurrentWaypoint();
                mover.SetSpeed(patrolSpeed);
            }

            if (timeSinceStop > dwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.transform.GetChild(currentWaypointIndex).position;
        }

        private void CycleWaypont()
        {
            if (currentWaypointIndex < patrolPath.transform.childCount - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex = 0;
            }
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(GetCurrentWaypoint(), transform.position) < 0.1;
        }


        private void SuspiciousBehaviour()
        {
            GetComponent<ActionSceduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            mover.SetSpeed(runSpeed);
            fighter.Atack(player);
            timeSinceLastSawPlayer = 0;
            Agreevate();
            AgreevateNearbyEnemyes();
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }
        
        //caled by unity 
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, visionDistance);
        }
    }
}
