using RPG.Atrributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
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

        private void Start()
        {
            fighter = GetComponent<Fighter>();
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

            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    mover.StartMoveAction(target);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(CursorRay(), out hit);
            if (isHit)
            {
                target = hit.point;
                NavMeshHit newHit;
                if (NavMesh.SamplePosition(hit.point, out newHit, 1.0f, NavMesh.AllAreas))
                {
                    Vector3 result = newHit.position;
                    return true;
                }
                return false;
            }
            target = new Vector3();
            return false;
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