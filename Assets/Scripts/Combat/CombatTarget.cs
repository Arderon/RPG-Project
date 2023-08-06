using RPG.Atrributes;
using UnityEngine;
using RPG.Control;
using RPG.Combat;
using UnityEditor.TerrainTools;

namespace RPG.Core
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IReycastable
    {
        public bool HandleRaycast(PlayerController playerController)
        {
            if (!playerController.GetComponent<Fighter>().CanAttack(gameObject)) 
            {
                return false;
            }
            if (Input.GetMouseButtonDown(1)) 
            {
                playerController.GetComponent<Fighter>().Atack(gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}