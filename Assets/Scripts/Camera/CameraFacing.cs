using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Camera
{
    public class CameraFacing : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(UnityEngine.Camera.main.transform.position);
        }
    }

}