using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if (isPlayed) return;
            if (other.gameObject.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
        }
    }
}
