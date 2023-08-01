using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;

        void LateUpdate()
        {
            gameObject.transform.position = target.transform.position;
        }
    }

}
