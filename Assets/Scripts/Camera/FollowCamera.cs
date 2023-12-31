using UnityEngine;

namespace RPG.Camera
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] UnityEngine.Camera cam;
        [SerializeField] float rotationSpeed;
        private Vector3 previousPosition;



        void LateUpdate()
        {
            gameObject.transform.position = target.transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = previousPosition - UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition);
                cam.transform.RotateAround(gameObject.transform.position, new Vector3(0, 1, 0), -direction.x * rotationSpeed);

                previousPosition = UnityEngine.Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }

}
