using UnityEngine;
using RPG.Stats;
using TMPro;

namespace RPG.HUD
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelText;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            int level = player.GetComponent<BaseStats>().GetLevel(); 
            levelText.text = level.ToString();
        }
    }
}

