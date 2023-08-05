using RPG.Atrributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
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

        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI
        }

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

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithUi()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        bool InteractWithMovement()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                return false;
            }
            RaycastHit hit;
            bool isHit = Physics.Raycast(CursorRay(), out hit);
            if (isHit)
            {
                if (Input.GetMouseButton(1))
                {
                    /*                    GetComponent<Fighter>().Cancel();*/
                    mover.StartMoveAction(hit.point);

                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(CursorRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;


                if (!fighter.CanAttack(target.gameObject))
                {
                    continue;
                }
                if (Input.GetMouseButton(1))
                {
                    fighter.Atack(target.gameObject);
                }
                SetCursor(CursorType.Combat);
                return true;
            }
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

