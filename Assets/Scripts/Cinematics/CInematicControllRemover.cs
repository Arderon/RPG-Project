using RPG.Combat;
using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinematic
{
    public class CInematicControllRemover : MonoBehaviour
    {
        GameObject player;
        PlayableDirector director;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            director = GetComponent<PlayableDirector>();
            director.played += DisableControl;           
            director.stopped += EnableControl;
        }

        public void EnableControl(PlayableDirector aDirector)
        {
            player.GetComponent<PlayerController>().EnableControlls();
            print("Controll enabled");
        }

        public void DisableControl(PlayableDirector aDirector)
        {
            player.GetComponent<ActionSceduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().DisableControlls();
            print("ControllDisabled");
        }
    }
}