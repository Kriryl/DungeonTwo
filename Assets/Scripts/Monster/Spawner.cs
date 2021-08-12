using UnityEngine;
using DungeonEscape.Debugging;

namespace DungeonEscape.Monsters
{
    public class Spawner : MonoBehaviour
    {
        private Debugger debugger;

        private void Start()
        {
            debugger = FindObjectOfType<Debugger>();    
        }

        /// <summary>
        /// Spawns a monster.
        /// </summary>
        public Monster Spawn(Monster monster, Transform target, Vector2 position)
        {
            Monster newMonster = Instantiate(monster, position, transform.rotation);
            newMonster.EnemyAI.target = target;
            debugger.ShowMonsterInfo();
            return newMonster;
        }
    }
}
