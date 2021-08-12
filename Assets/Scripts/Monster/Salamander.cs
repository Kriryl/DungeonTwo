using UnityEngine;
using UnityEngine.AI;
using Spine.Unity;

namespace DungeonEscape.Monsters
{
    [RequireComponent(typeof(Monster))]

    public class Salamander : MonoBehaviour
    {
        Monster monster;

        private void Start()
        {
            monster = GetComponent<Monster>();
            monster.IsWalking = true;
        }
    }
}
