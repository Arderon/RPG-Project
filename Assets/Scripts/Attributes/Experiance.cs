using RPG.Saving;
using UnityEngine;

namespace RPG.Atrributes
{
    public class Experiance : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiancePoints = 0;

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            print(experiancePoints);
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
