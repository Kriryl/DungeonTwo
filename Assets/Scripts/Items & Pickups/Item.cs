using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DungeonEscape.Interactibles;
using DungeonEscape.Enums;
using DungeonEscape.PlayerController;

namespace DungeonEscape
{
    [RequireComponent(typeof(SpriteRenderer))]

    /// <summary>
    /// Base class for all items.
    /// </summary>
    public partial class Item : MonoBehaviour
    {
        public PickupType itemType = PickupType.none;
        public WeaponType weaponType = WeaponType.none;
        public Sprite itemSprite;
        public Sword sword = new Sword();

        /// <summary>
        /// Clones the item and returns it.
        /// </summary>
        public GameObject Clone(Transform parent)
        {
            GameObject clone = Instantiate(this, parent).gameObject;
            return clone;
        }

        /// <summary>
        /// Shows the gameobject.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Puts the item in the specified chest.
        /// </summary>

        public void PutInChest(Chest chest)
        {
            gameObject.SetActive(false);
            transform.position = chest.transform.position;
            transform.SetParent(chest.transform);
            transform.localPosition = new Vector3(0, 0.25f);
        }

        /// <summary>
        /// This method is called when the player touches the item.
        /// </summary>
        public void OnPickup(Player player)
        {
            if (itemType == PickupType.equipment)
            {
                if (weaponType == WeaponType.sword)
                {
                    player.ApplySword(sword);
                }
            }
            player.AddToInventory(itemSprite, itemType, this);
            Destroy(gameObject);
        }
    }
}
