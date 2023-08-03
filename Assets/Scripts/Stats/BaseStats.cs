using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;


        private void Update()
        {
            if (gameObject.CompareTag("Player"))
            {
                print(GetLevel());
            }
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat,characterClass, GetLevel());
        }

        public int GetLevel()
        {
            Experiance experience = GetComponent<Experiance>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiance();
            int penultimateLevel = progression.GetLevels(Stat.ExperianceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperianceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
