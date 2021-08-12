using UnityEngine;
using DungeonEscape.Monsters;
using System.Collections.Generic;
using DungeonEscape.PlayerController;
using TMPro;

namespace DungeonEscape.Debugging
{
    public class Debugger : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public float playerDamage = 1f;
        public List<Monster> monsters = new List<Monster>();
        public List<float> monsterHealths = new List<float>();

        private Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            playerDamage = player.Damage;
            ShowMonsterInfo();
        }

        public void ShowMonsterInfo()
        {
            monsters.Clear();
            monsterHealths.Clear();
            Monster[] allMonsters = FindObjectsOfType<Monster>();
            foreach (Monster monster in allMonsters)
            {
                monsters.Add(monster);
                monsterHealths.Add(monster.CurrentHealth);
            }
        }

        private void Update()
        {
            playerDamage = player.Damage;

            string allMonsters = "Monsters: ";
            for (int i = 0; i < monsterHealths.Count; i++)
            {
                monsterHealths[i] = monsters[i].CurrentHealth;
                allMonsters += monsters[i].MonsterName;
                allMonsters += ", " + monsterHealths[i].ToString() + " HP.";
            }
            text.text = allMonsters;
        }
    }
}
