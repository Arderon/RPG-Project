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
        [SerializeField] const float speed = 5.66f;
        [SerializeField] float navPathMaxLength;
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status == NavMeshPathStatus.PathPartial) return false;
            if (GetPathLength(path) > navPathMaxLength) return false;
            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            if (path.corners.Length < 2) return 0;

            float lengthSoFar = 0.0F;
            for (int i = 1; i < path.corners.Length; i++)
            {
                lengthSoFar += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
            return lengthSoFar;
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

