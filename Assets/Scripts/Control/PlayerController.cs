using RPG.Atrributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Health health;
        private bool isEnabled = true;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMapping = null;
        [SerializeField] float navPathMaxLength;

        private void Start()
        {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }
        void Update()
        {

            if (!isEnabled) return;
            if (InteractWithUi())
            {
                SetCursor(CursorType.UI);
                return;
            }
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithUi()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = SortedHits();
            foreach (RaycastHit hit in hits)
            {
                IReycastable[] reycastables = hit.transform.GetComponents<IReycastable>();
                foreach (IReycastable reycastable in reycastables)
                {
                    if (reycastable.HandleRaycast(this))
                    {
                        SetCursor(reycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] SortedHits()
        {
            RaycastHit[] hits = Physics.RaycastAll(CursorRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[0] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        bool InteractWithMovement()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                return false;
            }
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (!hasHit) return false;

            if (Input.GetMouseButton(1))
            {
                mover.StartMoveAction(target);
            }
            SetCursor(CursorType.Movement);
            return true;

            
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool isHit = Physics.Raycast(CursorRay(), out hit);

            if (!isHit) return false;

            target = hit.point;
            NavMeshHit newHit;
            bool hasCustToNavMesh = NavMesh.SamplePosition(hit.point, out newHit, 1.0f, NavMesh.AllAreas);
            if (!hasCustToNavMesh) return false;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
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

        private void SetCursor(CursorType type)
        {
            if (cursorMapping == null) return;
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMapping)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMapping[0];
        }

        private Ray CursorRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void EnableControlls()
        {
            isEnabled = true;
        }

        public void DisableControlls()
        {
            isEnabled = false;
        }
    }
}