using UnityEngine;

namespace RPG.Atrributes
{
    public class Experiance : MonoBehaviour
    {
        [SerializeField] float experiancePoints = 0;

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            print(experiancePoints);
        }
    }
}
