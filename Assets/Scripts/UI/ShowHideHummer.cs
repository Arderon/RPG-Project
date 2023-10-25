using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI 
{
    public class ShowHideHummer : MonoBehaviour
    {
        [SerializeField] KeyCode key;
        [SerializeField] GameObject panel;

        void Start()
        {
            panel.SetActive(false);
        }


        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                panel.SetActive(!panel.activeSelf);
            }
        }
    }

}