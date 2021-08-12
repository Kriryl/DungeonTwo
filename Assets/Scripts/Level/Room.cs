using System.Collections.Generic;
using UnityEngine;
using DungeonEscape.Monsters;
using DungeonEscape.PlayerController;
using System.Collections;

namespace DungeonEscape
{
    public class Room : MonoBehaviour
    {
        public List<Monster> monsters = new List<Monster>();
        public List<Monster> spawnedMonsters = new List<Monster>();
        public List<GameObject> doors = new List<GameObject>();

        public int MonsterCount => monsters.Count;
        public int RemainingMonsters => spawnedMonsters.Count;

        Spawner spawner;
        Player player;

        private void Start()
        {
            spawner = FindObjectOfType<Spawner>();
            player = FindObjectOfType<Player>();
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }
        }

        public IEnumerator CloseAllDoors(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (RemainingMonsters > 0)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }
        }

        public void OnMonsterDead(Monster monster)
        {
            spawnedMonsters.Remove(monster);
            print($"{monster.MonsterName} is dead!");
            if (RemainingMonsters <= 0)
            {
                OpenAllDoors();
            }
        }

        private void OpenAllDoors()
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }
        }

        public void OnRoomEnter()
        {
            foreach (Monster monster in monsters)
            {
                Monster newMonster = spawner.Spawn(monster, player.transform, transform.position);
                newMonster.MonsterRoom = this;
                spawnedMonsters.Add(newMonster);
            }
            monsters.Clear();
        }
    }
}
