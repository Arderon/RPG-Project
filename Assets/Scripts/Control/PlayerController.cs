using RPG.Core;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using RPG.Atrributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter fighter;
        private Mover mover;
        private Health health;
        private bool isEnabled = true;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (!isEnabled) return;
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        bool InteractWithMovement()
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(CursorRay(), out hit);
            if (isHit)
            {
                if (Input.GetMouseButton(1))
                {
/*                    GetComponent<Fighter>().Cancel();*/
                    mover.StartMoveAction(hit.point);

                }
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
                return true;
            }
            return false;
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

