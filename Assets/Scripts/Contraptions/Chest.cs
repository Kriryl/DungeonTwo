using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonEscape.Interactibles
{
    public class Chest : MonoBehaviour
    {
        public Item content;
        /// <summary>
        /// Returns the item contained in the chest.
        /// </summary>
        public Item Contains => content;
        public GameObject chestClosed;
        public GameObject chestOpen;
        public bool IsOpened { get; set; } = false;

        private void Start()
        {
            content.PutInChest(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Open();
            }
        }

        /// <summary>
        /// Opens the chest. Note: This method is also called when the player collides with the chest.
        /// </summary>

        public void Open()
        {
            if (!IsOpened)
            {
                chestClosed.SetActive(false);
                chestOpen.SetActive(true);
                RevealItem();
                IsOpened = true;
            }
        }

        private void RevealItem()
        {
            content.Show();
        }
    }
}
