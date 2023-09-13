using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AsignToTransparantLayer : MonoBehaviour
    {
        [SerializeField] int layer;
        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer = layer;
            }
        }
    }

}