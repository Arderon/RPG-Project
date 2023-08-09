using System;
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
        [SerializeField] GameObject levelUpPrefab = null;
        [SerializeField] bool shouldUseModifiers = false;

        private GameObject levelUpEffect;
        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experiance experiance = GetComponent<Experiance>();
            if(experiance != null )
            {
                experiance.onExperianceGained += UpdateLevel;
            }
        }

        private void Update()
        {
            UpdateLevelUpEffect();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                PlayLevelUpEffect();
            }
        }

        private void PlayLevelUpEffect()
        {
            levelUpEffect = Instantiate(levelUpPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }

        private void UpdateLevelUpEffect()
        {
            if (levelUpEffect == null) return;
            levelUpEffect.transform.position = gameObject.transform.position;
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }


        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private int CalculateLevel()
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
