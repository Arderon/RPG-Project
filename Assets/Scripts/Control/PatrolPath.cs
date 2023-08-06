using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointSize = 1.5f;
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform waypoint = transform.GetChild(i);
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(waypoint.transform.position, waypointSize);
                Gizmos.DrawLine(waypoint.transform.position, transform.GetChild(GetNextPoint(i)).position);
            }
        }

        public int GetNextPoint(int i)
        {
            if (i < transform.childCount - 1)
            {
                return i + 1;
            }else
            {
                return 0;
            }
        }
    }
}