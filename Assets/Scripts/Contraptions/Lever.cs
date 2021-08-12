using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonEscape.Trigger
{
    public class Lever : MonoBehaviour
    {
        public GameObject leverActive;
        public GameObject leverDeactive;
        public GameObject trigger;

        public GameObject Trigger => trigger;

        public bool IsActive { get; set; } = false;

        private void Start()
        {
            leverActive.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Toggle();
        }

        public void Toggle()
        {
            IsActive = !IsActive;
            if (IsActive)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        public void Deactivate()
        {
            leverActive.SetActive(false);
            leverDeactive.SetActive(true);
            LookForTriggerAndClose();
        }

        public void Activate()
        {
            leverActive.SetActive(true);
            leverDeactive.SetActive(false);
            LookForTriggerAndOpen();
        }

        private void LookForTriggerAndClose()
        {
            Door door = Trigger.GetComponent<Door>();
            if (door)
            {
                door.Close();
            }
        }

        private void LookForTriggerAndOpen()
        {
            Door door = Trigger.GetComponent<Door>();
            if (door)
            {
                door.Open();
            }
        }
    }
}
