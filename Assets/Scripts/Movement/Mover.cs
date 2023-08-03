using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using System.ComponentModel;
using RPG.Atrributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] const float speed = 5f;
        private NavMeshAgent meshAgent;

        private void Start()
        {
            meshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (GetComponent<Health>().IsDead())
            {
                meshAgent.enabled= false;
            }
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionSceduler>().StartAction(this);
/*            GetComponent<Fighter>().Cancel();*/
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            meshAgent.destination = destination;
            meshAgent.isStopped = false;
        }

        public void Stop()
        {
            if (!meshAgent.isActiveAndEnabled) return;
            meshAgent.isStopped = true;
        }

        public void SetSpeed(float newSpeed = speed)
        {
            meshAgent.speed = newSpeed;
        }

        public void Cancel()
        {
            if (!GetComponent<Health>().IsDead())
            {
                meshAgent.isStopped = true;
            }
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = meshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            GetComponent<Animator>().SetFloat("forwardSpeed", localVelocity.z);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

