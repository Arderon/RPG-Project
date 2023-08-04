using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experiance : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiancePoints = 0;

        public event Action onExperianceGained;
        public event Action onLevelUp;

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            onExperianceGained();
        }

        public float GetExperiance()
        {
            return experiancePoints;
        }

        public object CaptureState()
        {
            return experiancePoints;
        }

        public void RestoreState(object state)
        {
            experiancePoints = (float)state;
        }
    }
}
