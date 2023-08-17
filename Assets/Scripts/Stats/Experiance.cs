using RPG.Atrributes;
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
        private BaseStats stats;

        private void Start()
        {
            stats = GetComponent<BaseStats>();
        }

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            onExperianceGained();
        }

        public void LevelUp()
        {
            onLevelUp();
        }

        public float GetExperiance()
        {
            return experiancePoints;
        }
        public float GetPercentage()
        {
            if (stats.GetLevel() == 1)
            {
                return experiancePoints / stats.GetStat(Stat.ExperianceToLevelUp);
            }
            return (experiancePoints - stats.GetBaseStatPrevious(Stat.ExperianceToLevelUp)) / (stats.GetStat(Stat.ExperianceToLevelUp) - stats.GetBaseStatPrevious(Stat.ExperianceToLevelUp));
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
